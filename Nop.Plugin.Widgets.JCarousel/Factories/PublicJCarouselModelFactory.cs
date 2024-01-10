using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Widgets.JCarousel.Domain;
using Nop.Plugin.Widgets.JCarousel.Models.Configuration;
using Nop.Plugin.Widgets.JCarousel.Services;
using Nop.Services.Catalog;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Web.Factories;
using Nop.Web.Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nop.Services.Stores;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Services.Seo;

namespace Nop.Plugin.Widgets.JCarousel.Factories
{
    public partial class PublicJCarouselModelFactory : IPublicJCarouselModelFactory
    {
        #region Fields
        private readonly IJCarouselService _jCarouselService;
        private readonly IProductService _productService;
        private readonly IProductModelFactory _productModelFactory;
        private readonly IStoreContext _storeContext;
        private readonly IStaticCacheManager _staticCacheManager;
        private readonly IAclService _aclService;
        private readonly CatalogSettings _catalogSettings;
        private readonly IOrderReportService _orderReportService;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IRecentlyViewedProductsService _recentlyViewedProductsService;
        private readonly IUrlRecordService _urlRecordService;
        private readonly ICategoryService _categoryService;
        private readonly IWorkContext _workContext;
        #endregion

        #region Ctor

        public PublicJCarouselModelFactory(
            IJCarouselService jCarouselService,
            IProductService productService,
            IProductModelFactory productModelFactory,
            IStaticCacheManager staticCacheManager,
            IStoreContext storeContext,
            IAclService aclService,
            CatalogSettings catalogSettings,
            IOrderReportService orderReportService,
            IStoreMappingService storeMappingService,
            IRecentlyViewedProductsService recentlyViewedProductsService,
            IUrlRecordService urlRecordService,
            ICategoryService categoryService,
            IWorkContext workContext)
        {
            _jCarouselService = jCarouselService;
            _productService = productService;
            _productModelFactory = productModelFactory;
            _staticCacheManager = staticCacheManager;
            _storeContext = storeContext;
            _aclService = aclService;
            _catalogSettings = catalogSettings;
            _orderReportService = orderReportService;
            _storeMappingService = storeMappingService;
            _recentlyViewedProductsService = recentlyViewedProductsService;
            _urlRecordService = urlRecordService;
            _categoryService = categoryService;
            _workContext = workContext;
        }

        #endregion

        #region Utilities
        /// <summary>
        /// Prepare list of Jcarousel Products mapping
        /// </summary>
        /// <param name="jcarousels">List of Jcarousels</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task<IList<Product>> PrepareJCarouselProductsModelAsync(JCarouselLog jcarousel)
        {
            if (jcarousel == null)
                throw new ArgumentNullException(nameof(jcarousel));

            var productIds = await _jCarouselService.GetProductIdsByJcarouselIdAsync(jcarousel.Id);

            return await _productService.GetProductsByIdsAsync(productIds.ToArray());
        }
        #endregion

        #region Methods
        public virtual async Task<IList<JCarouselModel>> PrepareJcarouselOverviewModelsAsync(IEnumerable<JCarouselLog> jcarousels)
        {
            if (jcarousels == null)
                throw new ArgumentNullException(nameof(jcarousels));
            var models = new List<JCarouselModel>();
            foreach (var jcarousel in jcarousels)
            {
                var jcarouselModel = jcarousel.ToModel<JCarouselModel>();
                jcarouselModel.SelectedCategoryId = jcarousel.CategoryId;
                var category = await _categoryService.GetCategoryByIdAsync(jcarouselModel.SelectedCategoryId);
                jcarouselModel.SeName = await _urlRecordService.GetSeNameAsync(category);
                var productnew = await PrepareJCarouselProductsModelAsync(jcarousel);
                //To get the current store
                var store = await _storeContext.GetCurrentStoreAsync();
                var allproducts = new List<Product>();
                //Data source of the selected jcaousel id
                var alldatasourceids = _jCarouselService.GetAllDatasource(jcarousel.Id);
                foreach (var datasourceid in alldatasourceids)
                {
                    //To get the name of the Data source from its Id
                    var dataSourceType = (DataSourceType)datasourceid;
                    switch (dataSourceType)
                    {
                        case DataSourceType.None:
                            allproducts = (List<Product>)await PrepareJCarouselProductsModelAsync(jcarousel);
                            break;

                        case DataSourceType.BestSellersProducts:
                            //Products of the selected jcarousel id
                            var report = await _staticCacheManager.GetAsync(
                              _staticCacheManager.PrepareKeyForDefaultCache(NopModelCacheDefaults.HomepageBestsellersIdsKey,
                              store),
                              async () => await (await _orderReportService.BestSellersReportAsync(
                              storeId: store.Id,
                              pageSize: _catalogSettings.NumberOfBestsellersOnHomepage)).ToListAsync());
                            //load products
                            var productsbest = await (await _productService.GetProductsByIdsAsync(report.Select(x => x.ProductId).ToArray()))
                            //ACL and store mapping
                            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
                            //availability dates
                            .Where(p => _productService.ProductIsAvailable(p)).ToListAsync();
                            allproducts = productnew.Concat(productsbest).Distinct().ToList();
                            if (!allproducts.Any())
                                return null;
                            break;

                        case DataSourceType.BestSellersProductsByQuantity:
                            var reportnew = await _staticCacheManager.GetAsync(
                            _staticCacheManager.PrepareKeyForDefaultCache(NopModelCacheDefaults.HomepageBestsellersIdsKey,
                                store),
                            async () => await (await _orderReportService.BestSellersReportAsync(
                                storeId: store.Id,
                                pageSize: _catalogSettings.NumberOfBestsellersOnHomepage, orderBy: OrderByEnum.OrderByQuantity)).ToListAsync());
                            //load products
                            var productsbestquantity = await (await _productService.GetProductsByIdsAsync(reportnew.Select(x => x.ProductId).ToArray()))
                            //ACL and store mapping
                            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
                            //availability dates
                            .Where(p => _productService.ProductIsAvailable(p)).ToListAsync();
                            allproducts = productnew.Concat(productsbestquantity).Distinct().ToList();
                            if (!allproducts.Any())
                                return null;
                            break;

                        case DataSourceType.MarkedAsNewProducts:
                            var storeIdnew = store.Id;
                            var newProducts = (List<Product>)await _productService.GetProductsMarkedAsNewAsync(storeIdnew);
                            allproducts = productnew.Concat(newProducts).Distinct().ToList();
                            if (!allproducts.Any())
                                return null;
                            break;

                        case DataSourceType.RecentlyViewedProducts:
                            var products = await (await _recentlyViewedProductsService.GetRecentlyViewedProductsAsync(_catalogSettings.RecentlyViewedProductsNumber))
                            //ACL and store mapping
                            .WhereAwait(async p => await _aclService.AuthorizeAsync(p) && await _storeMappingService.AuthorizeAsync(p))
                            //availability dates
                            .Where(p => _productService.ProductIsAvailable(p)).ToListAsync();
                            allproducts = productnew.Concat(products).Distinct().ToList();
                            if (!allproducts.Any())
                                return null;
                            break;

                        default:
                            allproducts = (List<Product>)await PrepareJCarouselProductsModelAsync(jcarousel);
                            break;
                    }
                    //Max items in a jcarousel selected from admin side
                    allproducts = (allproducts.Take(jcarousel.MaxItems)).ToList();
                    jcarouselModel.JcarouselProductsModel.Products.AddRange((await _productModelFactory.PrepareProductOverviewModelsAsync(allproducts)).ToList());
                };
                models.Add(jcarouselModel);
            }
            return models;
        }
        #endregion
    }
}
