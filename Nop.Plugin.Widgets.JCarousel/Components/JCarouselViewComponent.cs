using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widgets.JCarousel.Domain;
using Nop.Plugin.Widgets.JCarousel.Factories;
using Nop.Plugin.Widgets.JCarousel.Services;
using Nop.Web.Framework.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.JCarousel.Components
{
    /// <summary>
    /// Represents the view component to place a widget into pages
    /// </summary>
    [ViewComponent(Name = JCarouselDefaults.VIEW_COMPONENT)]
    public class JCarouselViewComponent : NopViewComponent
    {
        #region Fields
        private readonly IJCarouselService _jCarouselService;
        private readonly IPublicJCarouselModelFactory _publicjCarouselModelFactory;
        #endregion

        #region Ctor

        public JCarouselViewComponent(
            IJCarouselService jCarouselService,
            IPublicJCarouselModelFactory publicjCarouselModelFactory
            )
        {
            _jCarouselService = jCarouselService;
            _publicjCarouselModelFactory = publicjCarouselModelFactory;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Invoke view component
        /// </summary>
        /// <param name="widgetZone">Widget zone name</param>
        /// <param name="additionalData">Additional data</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the view component result
        /// </returns>
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone)
        {
            //To get the id of the widet zone selected
            WidgetZoneType key = (WidgetZoneType)Enum.Parse(typeof(WidgetZoneType), widgetZone);
            var widgetId = (int)key;

            var widgetIdList = await _jCarouselService.GetAllWidgetZoneIdsListAsync();

            //To check wheather the widget id is present in the widget ids list or not
            if (!widgetIdList.Contains(widgetId))
                return Content("");
            
                //To get the list of jcarousel ids with corresponding widget id 
                var jcarouselIds = await _jCarouselService.GetJCaroselIdsByWidgetIdAsync(widgetId);

                var jcarousels = new List<JCarouselLog>();
                foreach (var id in jcarouselIds)
                {
                    var jcarousel = await _jCarouselService.GetJcarouselByIdAsync(id);
                    jcarousels.Add(jcarousel);
                }

            //prepare model
            var model = await _publicjCarouselModelFactory.PrepareJcarouselOverviewModelsAsync(jcarousels);
                return View(model);
            

        }
        #endregion
    }
}
