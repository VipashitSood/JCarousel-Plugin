using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Web.Framework.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.JCarousel.Domain
{
    public partial class WidgetJCarouselMapping : BaseEntity 
    {
        public int JCarouselId { get; set; }
        public int WidgetId { get; set; }
        public int WidgetDisplayOrder { get; set; }
        public WidgetZoneType WidgetZoneType
        {
            get => (WidgetZoneType)WidgetId;
            set => WidgetId = (int)value;
        }
    }
}
