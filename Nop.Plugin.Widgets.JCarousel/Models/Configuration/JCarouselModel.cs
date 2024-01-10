using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Nop.Plugin.Widgets.JCarousel.Models.Configuration
{
    public record JCarouselModel : BaseNopEntityModel
    {
        public JCarouselModel()
        {
            AvailableDataSourceType = new List<SelectListItem>();
            JCarouselProductSearchModel = new JCarouselProductSearchModel();
            JCarouselWidgetSearchModel = new JCarouselWidgetSearchModel();

            JcarouselProductsModel = new JcarouselProductsModel();
            AvailableCategories = new List<SelectListItem>();
        }
        
        [NopResourceDisplayName("Plugins.Widgets.JCarousel.Name")]
        public string Name { get; set; }
       
        [NopResourceDisplayName("Plugins.Widgets.JCarousel.DataSourceName")]
        public string DataSourceName { get; set; }
        
        [NopResourceDisplayName("Plugins.Widgets.JCarousel.MaxItems")]
        public int MaxItems { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.DotNevigation")]
        public bool DotNevigation { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.ArrowNevigation")]
        public bool ArrowNevigation { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.DataSourceTypeId")]
        public int DataSourceTypeId { get; set; }

        public IList<SelectListItem> AvailableDataSourceType { get; set; }
        public JCarouselProductSearchModel JCarouselProductSearchModel { get; set; }
        public JCarouselWidgetSearchModel JCarouselWidgetSearchModel { get; set; }
        public int JCarouselId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.WidgetZoneId")]
        public int WidgetId { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.WidgetZoneName")]
        public string WidgetZoneName { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.WidgetDisplayOrder")]
        public int WidgetDisplayOrder { get; set; }

        [NopResourceDisplayName("Plugins.Widgets.JCarousel.DisplayOrder")]
        public int DisplayOrder { get; set; }

        public JcarouselProductsModel JcarouselProductsModel { get; set; }
        //category
        [NopResourceDisplayName("Plugins.Widgets.JCarousel.SelectedCategoryId")]
        public int SelectedCategoryId { get; set; }
        public string SeName { get; set; }
        public IList<SelectListItem> AvailableCategories { get; set; }
    }
}
