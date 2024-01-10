using System;
using System.Linq;

namespace Nop.Plugin.Widgets.JCarousel.Domain
{
    /// <summary>
    /// Enum list to select the widget zones in the Jcarousel slider
    /// </summary>

    public enum WidgetZoneType
    {
        home_page_top = 1,
        home_page_before_best_sellers=2,
        home_page_before_categories =3,
        home_page_before_news=4,
        home_page_before_poll=5,
        home_page_before_products=6,
        home_page_bottom=7,
        categorydetails_top=8,
        categorydetails_bottom=9,
        productdetails_top=10,
        productdetails_bottom=11,
        manufacturerdetails_top=12,
        manufacturerdetails_bottom=13,
        left_side_column_before=14,
        left_side_column_after=15,
        right_side_column_before=16,
        right_side_column_after=17
    }
}
