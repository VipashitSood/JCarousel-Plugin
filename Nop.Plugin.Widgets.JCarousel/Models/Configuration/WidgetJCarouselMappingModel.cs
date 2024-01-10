using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.JCarousel.Models.Configuration
{
    public partial record WidgetJCarouselMappingModel : BaseNopEntityModel 
    {
        public WidgetJCarouselMappingModel()
        {
            AvailableWidgetZoneType = new List<SelectListItem>();
        }
        public int JCarouselId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.WidgetZoneId")]
        public int WidgetId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.WidgetZoneName")]
        public string WidgetZoneName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.WidgetDisplayOrder")]
        public int WidgetDisplayOrder { get; set; }
        public IList<SelectListItem> AvailableWidgetZoneType { get; set; }
    }
}
