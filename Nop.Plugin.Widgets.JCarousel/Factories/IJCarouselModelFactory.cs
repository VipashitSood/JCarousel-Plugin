using Nop.Plugin.Widgets.JCarousel.Domain;
using Nop.Plugin.Widgets.JCarousel.Models.Configuration;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.JCarousel.Factories
{
    public partial interface IJCarouselModelFactory
    {
        /// <summary>
        /// Prepare JCarousel search model
        /// </summary>
        /// <param name="searchModel">JCarousel search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the JCarousel search model
        /// </returns>
        Task<JCarouselSearchModel> PrepareJCarouselSearchModelAsync(JCarouselSearchModel searchModel);

        /// <summary>
        /// Prepare paged JCarousel list model
        /// </summary>
        /// <param name="searchModel">JCarousel search model</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the JCarousel list model
        /// </returns>
        Task<JCarouselListModel> PrepareJCarouselListModelAsync(JCarouselSearchModel searchModel);

        /// <summary>
        /// Prepare JCarousel model
        /// </summary>
        /// <param name="model">JCarousel model</param>
        /// <param name="jacrousel">JCarousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the JCarousel model
        /// </returns>
        Task<JCarouselModel> PrepareJCarouselModelAsync(JCarouselModel model, JCarouselLog jcarousel, bool excludeProperties = false);

        /// <summary>
        /// Prepare paged jacrousel product list model
        /// </summary>
        /// <param name="searchModel">JCarousel product search model</param>
        /// <param name="jcarousel">JCarousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the jacrousel product list model
        /// </returns>
        Task<JCarouselProductListModel> PrepareJCarouselProductListModelAsync(JCarouselProductSearchModel searchModel, JCarouselLog jcarousel);

        /// <summary>
        /// Prepare product search model to add to the jacrousel
        /// </summary>
        /// <param name="searchModel">Product search model to add to the jacrousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product search model to add to the jacrousel
        /// </returns>
        Task<AddProductToJCarouselSearchModel> PrepareAddProductToJCarouselSearchModelAsync(AddProductToJCarouselSearchModel searchModel);

        /// <summary>
        /// Prepare paged product list model to add to the jacrousel
        /// </summary>
        /// <param name="searchModel">Product search model to add to the jacrousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the product list model to add to the jacrousel
        /// </returns>
        Task<AddProductToJCarouselListModel> PrepareAddProductToJCarouselListModelAsync(AddProductToJCarouselSearchModel searchModel);

        /// <summary>
        /// Prepare paged jacrousel widget list model
        /// </summary>
        /// <param name="searchModel">JCarousel widget search model</param>
        /// <param name="jcarousel">JCarousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the jacrousel widget list model
        /// </returns>
        Task<JCarouselWidgetListModel> PrepareJCarouselWidgetListModelAsync(JCarouselWidgetSearchModel searchModel, JCarouselLog jcarousel);

        /// <summary>
        /// Prepare widget search model to add to the jacrousel
        /// </summary>
        /// <param name="searchModel">Widget search model to add to the jacrousel</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the widget search model to add to the jacrousel
        /// </returns>
        Task<AddWidgetToJCarouselSearchModel> PrepareAddWidgetToJCarouselSearchModelAsync(AddWidgetToJCarouselSearchModel searchModel);
    }
}
