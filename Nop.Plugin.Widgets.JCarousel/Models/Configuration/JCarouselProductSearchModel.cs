using Nop.Web.Framework.Models;

namespace Nop.Plugin.Widgets.JCarousel.Models.Configuration
{
    public partial record JCarouselProductSearchModel : BaseSearchModel
    {
        #region Properties

        public int JCarouselId { get; set; }

        #endregion
    }
}
