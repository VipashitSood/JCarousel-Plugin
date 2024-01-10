using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Widgets.JCarousel.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.JCarousel.Services
{
    public partial class JCarouselService : IJCarouselService
    {
        #region Fields
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductJCarouselMapping> _productJcarouselRepository;
        private readonly IRepository<WidgetJCarouselMapping> _widgetJcarouselRepository;
        private readonly IRepository<JCarouselLog> _jCarousalRepository;
        private readonly IRepository<ProductJCarouselMapping> _productjCarousalRepository;
        #endregion
        #region Ctor

        public JCarouselService(
            IRepository<Product> productRepository,
            IRepository<ProductJCarouselMapping> productJcarouselRepository,
            IRepository<WidgetJCarouselMapping> widgetJcarouselRepository,
            IRepository<JCarouselLog> jCarousalRepository,
            IRepository<ProductJCarouselMapping> productjCarousalRepository)
        {
            _productRepository = productRepository;
            _productJcarouselRepository = productJcarouselRepository;
            _widgetJcarouselRepository = widgetJcarouselRepository;
            _jCarousalRepository = jCarousalRepository;
            _productjCarousalRepository = productjCarousalRepository;
        }

        #endregion

        #region Methods
        

        /// <summary>
        /// Inserts a Jcarousel
        /// </summary>
        /// <param name="jCarousel">Jcarousel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertJCarouselAsync(JCarouselLog jCarousel)
        {
            await _jCarousalRepository.InsertAsync(jCarousel);
        }

        /// <summary>
        /// Delete a Jcarousel
        /// </summary>
        /// <param name="jCarousel">Jcarousel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteJCarouselAsync(JCarouselLog jCarousel)
        {
            await _jCarousalRepository.DeleteAsync(jCarousel);
        }

        /// <summary>
        /// Gets jcarousel
        /// </summary>
        /// <param name="jCarouselId">Jcarousel identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product
        public virtual async Task<JCarouselLog> GetJCarouselByIdAsync(int jCarouselId)
        {
            return await _jCarousalRepository.GetByIdAsync(jCarouselId, cache => default);
        }

        /// <summary>
        /// Updates a Jcarousel
        /// </summary>
        /// <param name="jCarousel">Jcarousel</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateJCarouselAsync(JCarouselLog jCarousel)
        {
            await _jCarousalRepository.UpdateAsync(jCarousel);
        }

        /// <summary>
        /// Gets all jcarousels
        /// </summary>
        /// <param name="name">JCarousel name</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the reviews
        /// </returns>
        public virtual async Task<IPagedList<JCarouselLog>> GetAllJCarouselsAsync(string name = null,
            int pageIndex = 0, int pageSize = int.MaxValue,
            bool showHidden = false)
        {
            return await _jCarousalRepository.GetAllPagedAsync(query =>
            {
                if (!string.IsNullOrWhiteSpace(Name))
                    query = query.Where(a => a.Name.Contains(Name));
                query = query.Distinct().OrderByDescending(a => a.Id);
                return query.OrderBy(a => a.Id);
            }, pageIndex, pageSize);
        }

        /// <summary>
        /// Gets product jcarousel mapping collection
        /// </summary>
        /// <param name="jacrouselId">Jcarousel identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product a jcarousel mapping collection
        /// </returns>
        public virtual async Task<IPagedList<ProductJCarouselMapping>> GetProductJCarouselsByJCarouselIdAsync(int jacrouselId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (jacrouselId == 0)
                return new PagedList<ProductJCarouselMapping>(new List<ProductJCarouselMapping>(), pageIndex, pageSize);

            var query = from pm in _productJcarouselRepository.Table
                        join p in _productRepository.Table on pm.ProductId equals p.Id
                        where pm.JCarouselId == jacrouselId 
                        orderby pm.DisplayOrder, pm.Id
                        select pm;
            return await query.ToPagedListAsync(pageIndex, pageSize);
        }

        /// <summary>
        /// Gets a product jcarousel mapping 
        /// </summary>
        /// <param name="productJcarouselId">Product jcarousel mapping identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product category mapping
        /// </returns>
        public virtual async Task<ProductJCarouselMapping> GetProductJCarouselByIdAsync(int productJcarouselId)
        {
            return await _productJcarouselRepository.GetByIdAsync(productJcarouselId, cache => default);
        }

        /// <summary>
        /// Updates the product jcarousel mapping 
        /// </summary>
        /// <param name="productJcarousel">>Product jcarousel mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateProductJCarouselAsync(ProductJCarouselMapping productJcarousel)
        {
            await _productJcarouselRepository.UpdateAsync(productJcarousel);
        }

        /// <summary>
        /// Deletes the product jcarousel mapping 
        /// </summary>
        /// <param name="productJcarousel">>Product jcarousel mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteProductJCarouselAsync(ProductJCarouselMapping productJcarousel)
        {
            await _productJcarouselRepository.DeleteAsync(productJcarousel);
        }

        /// <summary>
        /// Returns a ProductJcarousel that has the specified values
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="jcarouselId">Jcarousel identifier</param>
        /// <returns>A ProductJcarousel that has the specified values; otherwise null</returns>
        public virtual ProductJCarouselMapping FindProductJCarousel(IList<ProductJCarouselMapping> source, int productId, int jcarouselId)
        {
            foreach (var productJcarousel in source)
                if (productJcarousel.ProductId == productId && productJcarousel.JCarouselId == jcarouselId)
                    return productJcarousel;
            return null;
        }

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="productJcarousel">Product jcarousel mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertProductJCarouselAsync(ProductJCarouselMapping productJcarousel)
        {
            await _productJcarouselRepository.InsertAsync(productJcarousel);
        }
        #endregion

        /// <summary>
        /// Gets widget jcarousel mapping collection
        /// </summary>
        /// <param name="jacrouselId">Jcarousel identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the widget jcarousel mapping collection
        /// </returns>
        public virtual async Task<IPagedList<WidgetJCarouselMapping>> GetWidgetJCarouselsByJCarouselIdAsync(int jacrouselId,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false)
        {
            if (jacrouselId == 0)
                return new PagedList<WidgetJCarouselMapping>(new List<WidgetJCarouselMapping>(), pageIndex, pageSize);

            var query = from pm in _widgetJcarouselRepository.Table
                        join p in _jCarousalRepository.Table on pm.JCarouselId equals p.Id
                        where pm.JCarouselId == jacrouselId
                        orderby pm.WidgetDisplayOrder, pm.Id
                        select pm;
            return await query.ToPagedListAsync(pageIndex, pageSize);
        }


        /// <summary>
        /// Gets Widget jcarousel mapping
        /// </summary>
        /// <param name="widgetJcarouselId">Widget Jcarousel identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the Widget Jcarousel mapping
        /// </returns>
        public virtual async Task<WidgetJCarouselMapping> GetWidgetJCarouselByIdAsync(int widgetJcarouselId)
        {
            return await _widgetJcarouselRepository.GetByIdAsync(widgetJcarouselId, cache => default);
        }

        /// <summary>
        /// Inserts a Widget Jcarousel
        /// </summary>
        /// <param name="widgetJcarousel">Widget Jcarousel mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task InsertWidgetJCarouselAsync(WidgetJCarouselMapping widgetJcarousel)
        {
            await _widgetJcarouselRepository.InsertAsync(widgetJcarousel);
        }

        /// <summary>
        /// Updates the Widget jcarousel mapping 
        /// </summary>
        /// <param name="widgetJcarousel">>Widget jcarousel mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task UpdateWidgetJCarouselAsync(WidgetJCarouselMapping widgetJcarousel)
        {
            await _widgetJcarouselRepository.UpdateAsync(widgetJcarousel);
        }

        /// <summary>
        /// Deletes the Widget jcarousel mapping 
        /// </summary>
        /// <param name="widgetJcarousel">>Widget jcarousel mapping</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        public virtual async Task DeleteWidgetJCarouselAsync(WidgetJCarouselMapping widgetJcarousel)
        {
            await _widgetJcarouselRepository.DeleteAsync(widgetJcarousel);
        }

        /// <summary>
        /// Get count of Widget id with Jcarousel identifier and widget Identifier
        /// </summary>
        /// <param name="jCarouselId">Jcarousel identifier</param>
        /// <param name="widgetId">Widget identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public virtual int GetAllWidgets(int jCarouselId, int widgetId)
        {
            var query = from pm in _widgetJcarouselRepository.Table
                        where pm.JCarouselId == jCarouselId && pm.WidgetId == widgetId
                        select pm.WidgetId;

            return query.Count();
        }

        /// <summary>
        /// Get Data source of products with Jcarousel identifier 
        /// </summary>
        /// <param name="jCarouselId">Jcarousel identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public IList<int> GetAllDatasource(int jCarouselId)
        {
            var query = from pm in _jCarousalRepository.Table
                        where pm.Id == jCarouselId
                        select pm.DataSourceTypeId;

            return query.ToList();
        }
        
        /// <summary>
        /// Gets Jcarousel ids by Widget identifier
        /// </summary>
        /// <param name="widgetId">Widget identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pictures
        /// </returns>
        public virtual async Task<IList<int>> GetJCaroselIdsByWidgetIdAsync(int widgetId)
        {
            var query = (from pm in _widgetJcarouselRepository.Table
                         where pm.WidgetId == widgetId
                         select pm.JCarouselId).Distinct();

            return query.ToList();
        }


        /// <summary>
        /// Gets Product ids by jcarousel identifier
        /// </summary>
        /// <param name="jacrouselId">Jcarousel identifier</param>
        /// <param name="recordsToReturn">Number of records to return. 0 if you want to get all items</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the pictures
        /// </returns>
        public virtual async Task<IList<int>> GetProductIdsByJcarouselIdAsync(int jacrouselId, int recordsToReturn = 0)
        {
            if (jacrouselId == 0)
                return new List<int>();

            var query = from p in _productRepository.Table
                        join pp in _productjCarousalRepository.Table on p.Id equals pp.ProductId
                        orderby pp.DisplayOrder, pp.Id
                        where pp.JCarouselId == jacrouselId
                        select p.Id;
            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);
            var products = await query.ToListAsync();
            return products;
        }

        /// <summary>
        /// Gets a Jcarousel
        /// </summary>
        /// <param name="jcarouselId">Jcarousel identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the category
        /// </returns>
        public virtual async Task<JCarouselLog> GetJcarouselByIdAsync(int jcarouselId)
        {
            return await _jCarousalRepository.GetByIdAsync(jcarouselId, cache => default);
        }

        /// <summary>
        /// To get the list of names of Widget zones
        /// </summary>
        public async Task<IList<string>> GetWidgetZoneTypeListAsync()
        {
            return Enum.GetNames(typeof(WidgetZoneType)).ToList();
        }

        /// <summary>
        /// Gets the list of Widget zone ids
        /// </summary>
        public async Task<IList<int>> GetAllWidgetZoneIdsListAsync()
        {
            var query = from p in _widgetJcarouselRepository.Table
                        select p.WidgetId;
            return query.ToList();
        }

        /// <summary>
        /// Check and delete the widgets mapping with Jcarousel identifier
        /// </summary>
        /// <param name="jcarouselId">Jcarousel identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public async Task DeleteWidgetReferenceAsync(int jcarouselId)
        {

            var query = from p in _widgetJcarouselRepository.Table
                        where p.JCarouselId == jcarouselId
                        select p;
            var widgets = query.ToList();
            foreach (var widget in widgets)
            {
                await _widgetJcarouselRepository.DeleteAsync(widget);
            }
        }

        /// <summary>
        /// Check and delete the products mapping with Jcarousel identifier
        /// </summary>
        /// <param name="jcarouselId">Jcarousel identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the result
        /// </returns>
        public async Task DeleteProductReferenceAsync(int jcarouselId)
        {
            var query = from p in _productJcarouselRepository.Table
                        where p.JCarouselId == jcarouselId
                        select p;
            var Products = query.ToList();
            foreach (var Product in Products)
            {
                await _productJcarouselRepository.DeleteAsync(Product);
            }
        }

        /// <summary>
        /// Check if the Jcaousel name already exists
        /// </summary>
        /// <param name="name">Jcarousel name to be checked</param>
        public virtual string CheckExistingName(string name)
        {
            var jcarouselNames = from sci in _jCarousalRepository.Table
                                 where sci.Name == name
                                 select sci.Name;
            var emailList = jcarouselNames.FirstOrDefault();
            return emailList;
        }
    }
}
