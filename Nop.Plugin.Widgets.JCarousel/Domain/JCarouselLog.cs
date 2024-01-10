using Nop.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nop.Plugin.Widgets.JCarousel.Domain
{
    public class JCarouselLog : BaseEntity
    {
        public string Name { get; set; }
        public int DataSourceTypeId { get; set; }
        public int CategoryId { get; set; }
        public int MaxItems { get; set; }
        public int DisplayOrder { get; set; }
        public bool DotNevigation { get; set; }
        public bool ArrowNevigation { get; set; }
        public DataSourceType DataSourceType
        {
            get => (DataSourceType)DataSourceTypeId;
            set => DataSourceTypeId = (int)value;
        }
    }
}
