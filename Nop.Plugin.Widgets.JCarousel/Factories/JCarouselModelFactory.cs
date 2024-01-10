using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.JCarousel.Domain;
using Nop.Plugin.Widgets.JCarousel.Models.Configuration;
using Nop.Plugin.Widgets.JCarousel.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Models.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.JCarousel.Factories
{
    public partial class JCarouselModelFactory : IJCarouselModelFactory
    {
        #region Fields
        private readonly IBaseAdminModelFactory _baseAdminModelFactory;
        private readonly IJCarouselService _jCarouselService;
        private readonly ILocalizationService _localizationService;
        private readonly IProductService _productService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ICategoryService _categoryService;
        #endregion

        #region Ctor
        public JCarouselModelFactory(
            IBaseAdminModelFactory baseAdminModelFactory,
            IJCarouselService jCarouselService,
            ILocalizationService localizationService,
            IProductService productService,
            IUrlRecordService urlRecordService,
            ICategoryService categoryService)
        {
            _baseAdminModelFactory = baseAdminModelFactory;
            _jCarouselService = jCarouselService;
            _localizationService = localizationService;
            _productService = productService;
            _urlRecordService = urlRecordService;
            _categoryService = categoryService;
        }
        #endregion

        #region Utilities

        /// <summary>
        /// Prepare jacrousel product search model
        /// </summary>
        /// <param name="searchModel">Jcarousel product search model</param>
        /// <param name="jacrousel">Jcarousel</param>
        /// <returns>Jcarousel product search model</returns>
        protected virtual JCarouselProductSearchModel PrepareJcarouselProductSearchModel(JCarouselProductSearchModel searchModel,
            JCarouselLog jcarousel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            if (jcarousel == null)
                throw new ArgumentNullException(nameof(jcarousel));
            searchModel.JCarouselId = jcarousel.Id;
            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }

        /// <summary>
        /// Prepare jacrousel widget search model
        /// </summary>
        /// <param name="searchModel">Jcarousel widget search model</param>
        /// <param name="jacrousel">Jcarousel</param>
        /// <returns>Jcarousel widget search model</returns>
        protected virtual JCarouselWidgetSearchModel PrepareJcarouselWidgetSearchModel(JCarouselWidgetSearchModel searchModel,
            JCarouselLog jcarousel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            if (jcarousel == null)
                throw new ArgumentNullException(nameof(jcarousel));
            searchModel.JCarouselId = jcarousel.Id;
            //prepare page parameters
            searchModel.SetGridPageSize();
            return searchModel;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Prepare JCarousel search model
        /// </summary>
        /// <param name="searchModel">JCarousel search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the JCarousel search model
        /// </returns>
        public virtual async Task<JCarouselSearchModel> PrepareJCarouselSearchModelAsync(JCarouselSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare grid
            searchModel.SetGridPageSize();
            return searchModel;
        }

        /// <summary>
        /// Prepare paged JCarousel list model
        /// </summary>
        /// <param name="searchModel">JCarousel search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the JCarousel list model
        /// </returns>
        public virtual async Task<JCarouselListModel> PrepareJCarouselListModelAsync(JCarouselSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //get products
            var jcarousels = await _jCarouselService.GetAllJCarouselsAsync(
                Name: searchModel.SearchJCarouselName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize,
                showHidden: true);

            //prepare list model
            var model = await new JCarouselListModel().PrepareToGridAsync(searchModel, jcarousels, () =>
            {
                return jcarousels.SelectAwait(async jcarousal => 
                {
                    //Fill in model values from entity
                    var jcarouselModel = jcarousal.ToModel<JCarouselModel>();
                    jcarouselModel.SelectedCategoryId = jcarousal.CategoryId;
                    jcarouselModel.DataSourceTypeId = jcarousal.DataSourceTypeId;
                    jcarouselModel.DataSourceName = await _localizationService.GetLocalizedEnumAsync(jcarousal.DataSourceType);
                    return jcarouselModel;
                });
            });

            return model;
        }

        /// <summary>
        /// Prepare JCarousel model
        /// </summary>
        /// <param name="model">JCarousel model</param>
        /// <param name="jacrousel">JCarousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the JCarousel model
        /// </returns>
        public virtual async Task<JCarouselModel> PrepareJCarouselModelAsync(JCarouselModel model, JCarouselLog jcarosal, bool excludeProperties = false)
        {
            if (jcarosal != null)
            {
                //fill in model values from the entity
                if (model == null)
                {
                    //Fill in model values from entity
                    model = jcarosal.ToModel<JCarouselModel>();
                    model.SelectedCategoryId = jcarosal.CategoryId;
                    model.DataSourceTypeId = jcarosal.DataSourceTypeId;
                    model.DataSourceName = await _localizationService.GetLocalizedEnumAsync(jcarosal.DataSourceType);
                }
                var category = await _categoryService.GetCategoryByIdAsync(model.SelectedCategoryId);
                model.SeName = await _urlRecordService.GetSeNameAsync(category);

                //prepare nested search model
                PrepareJcarouselProductSearchModel(model.JCarouselProductSearchModel, jcarosal);

                //prepare nested search model
                PrepareJcarouselWidgetSearchModel(model.JCarouselWidgetSearchModel, jcarosal);
            }
            //set default values for the new model
            if (jcarosal == null)
            {
                model.MaxItems = 10;
            }
            await _baseAdminModelFactory.PrepareCategoriesAsync(model.AvailableCategories,
             defaultItemText: await _localizationService.GetResourceAsync("Plugins.Widgets.Jcarousel.Categories.Fields.Select"));
            return model;
        }

        /// <summary>
        /// Prepare paged jacrousel product list model
        /// </summary>
        /// <param name="searchModel">JCarousel product search model</param>
        /// <param name="jcarousel">JCarousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the jacrousel product list model
        /// </returns>
        public virtual async Task<JCarouselProductListModel> PrepareJCarouselProductListModelAsync(JCarouselProductSearchModel searchModel, JCarouselLog jcarousel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            if (jcarousel == null)
                throw new ArgumentNullException(nameof(jcarousel));
            //get product categories
            var productCategories = await _jCarouselService.GetProductJCarouselsByJCarouselIdAsync(jcarousel.Id,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);
            //prepare grid model
            var model = await new JCarouselProductListModel().PrepareToGridAsync(searchModel, productCategories, () =>
            {
                return productCategories.SelectAwait(async jcarouselProduct =>
                {
                    //fill in model values from the entity
                    var jcarouselProductModel = jcarouselProduct.ToModel<ProductJCarouselMappingModel>();
                    //fill in additional values (not existing in the entity)
                    jcarouselProductModel.ProductName = (await _productService.GetProductByIdAsync(jcarouselProduct.ProductId))?.Name;
                    return jcarouselProductModel;
                });
            });
            return model;
        }

        /// <summary>
        /// Prepare product search model to add to the jacrousel
        /// </summary>
        /// <param name="searchModel">Product search model to add to the jacrousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product search model to add to the jacrousel
        /// </returns>
        public virtual async Task<AddProductToJCarouselSearchModel> PrepareAddProductToJCarouselSearchModelAsync(AddProductToJCarouselSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare available categories
            await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);
            //prepare available manufacturers
            await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);
            //prepare available stores
            await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);
            //prepare available vendors
            await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);
            //prepare available product types
            await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);
            //prepare page parameters
            searchModel.SetPopupGridPageSize();
            return searchModel;
        }

        /// <summary>
        /// Prepare paged product list model to add to the jacrousel
        /// </summary>
        /// <param name="searchModel">Product search model to add to the jacrousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product list model to add to the jacrousel
        /// </returns>
        public virtual async Task<AddProductToJCarouselListModel> PrepareAddProductToJCarouselListModelAsync(AddProductToJCarouselSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //get products
            var products = await _productService.SearchProductsAsync(showHidden: true,
                manufacturerIds: new List<int> { searchModel.SearchManufacturerId },
                storeId: searchModel.SearchStoreId,
                vendorId: searchModel.SearchVendorId,
                productType: searchModel.SearchProductTypeId > 0 ? (ProductType?)searchModel.SearchProductTypeId : null,
                keywords: searchModel.SearchProductName,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);
            //prepare grid model
            var model = await new AddProductToJCarouselListModel().PrepareToGridAsync(searchModel, products, () =>
            {
                return products.SelectAwait(async product =>
                {
                    var productModel = product.ToModel<ProductModel>();
                    productModel.SeName = await _urlRecordService.GetSeNameAsync(product, 0, true, false);
                    return productModel;
                });
            });
            return model;
        }

        /// <summary>
        /// Prepare paged jacrousel widget list model
        /// </summary>
        /// <param name="searchModel">JCarousel widget search model</param>
        /// <param name="jcarousel">JCarousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the jacrousel widget list model
        /// </returns>
        public virtual async Task<JCarouselWidgetListModel> PrepareJCarouselWidgetListModelAsync(JCarouselWidgetSearchModel searchModel, JCarouselLog jcarousel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            if (jcarousel == null)
                throw new ArgumentNullException(nameof(jcarousel));
            //get widget jcarouels
            var productCategories = await _jCarouselService.GetWidgetJCarouselsByJCarouselIdAsync(jcarousel.Id,
                showHidden: true,
                pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

            //prepare grid model
            var model = await new JCarouselWidgetListModel().PrepareToGridAsync(searchModel, productCategories, () =>
            {
                return productCategories.SelectAwait(async jcarouselWidget =>
                {
                    //fill in model values from the entity
                    var jcarouselWidgetModel = new WidgetJCarouselMappingModel();
                    jcarouselWidgetModel.Id = jcarouselWidget.Id;
                    jcarouselWidgetModel.JCarouselId = jcarouselWidget.JCarouselId;
                    jcarouselWidgetModel.WidgetId = jcarouselWidget.WidgetId;
                    jcarouselWidgetModel.WidgetDisplayOrder = jcarouselWidget.WidgetDisplayOrder;
                    jcarouselWidgetModel.WidgetZoneName = await _localizationService.GetLocalizedEnumAsync(jcarouselWidget.WidgetZoneType);
                    return jcarouselWidgetModel;
                });
            });
            return model;
        }

        /// <summary>
        /// Prepare widget search model to add to the jacrousel
        /// </summary>
        /// <param name="searchModel">Widget search model to add to the jacrousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the widget search model to add to the jacrousel
        /// </returns>
        public virtual async Task<AddWidgetToJCarouselSearchModel> PrepareAddWidgetToJCarouselSearchModelAsync(AddWidgetToJCarouselSearchModel searchModel)
        {
            if (searchModel == null)
                throw new ArgumentNullException(nameof(searchModel));
            //prepare page parameters
            searchModel.SetPopupGridPageSize();
            return searchModel;
        }
        
        #endregion
    }
}
