using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.JCarousel.Models.Configuration
{
    public partial record JCarouselWidgetSearchModel : BaseSearchModel 
    {
        #region Properties

        public int JCarouselId { get; set; }

        #endregion
    }
}
