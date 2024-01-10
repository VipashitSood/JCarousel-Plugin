using Nop.Plugin.Widgets.JCarousel.Domain;
using Nop.Plugin.Widgets.JCarousel.Models.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.JCarousel.Factories
{
    public partial interface IPublicJCarouselModelFactory
    {
        /// <summary>
        /// Prepare Jcarousel public view model
        /// </summary>
        /// <param name="jcarousels">List of Jcarousels</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task<IList<JCarouselModel>> PrepareJcarouselOverviewModelsAsync(IEnumerable<JCarouselLog> jcarousels);
    }
}
