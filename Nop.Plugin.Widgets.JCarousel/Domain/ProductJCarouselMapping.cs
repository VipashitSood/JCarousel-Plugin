using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.JCarousel.Domain
{
    public partial class ProductJCarouselMapping : BaseEntity
    {
        public int ProductId { get; set; }
        public int JCarouselId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsFeaturedProduct { get; set; }
    }
}
