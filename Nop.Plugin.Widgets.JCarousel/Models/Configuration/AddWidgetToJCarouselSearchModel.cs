using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widgets.JCarousel.Models.Configuration
{
    public partial record AddWidgetToJCarouselSearchModel : BaseSearchModel 
    {
        [NopResourceDisplayName("Plugins.Widgets.JCarousel.Widgets.List.SearchJCarouselName")]
        public string SearchWidgetName { get; set; }
    }
}
