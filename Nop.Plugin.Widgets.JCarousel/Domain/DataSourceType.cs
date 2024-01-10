namespace Nop.Plugin.Widgets.JCarousel.Domain
{
    /// <summary>
    /// Enum list to select the products in the Jcarousel slider
    /// </summary>

    public enum DataSourceType
    {
        None = 0,
        BestSellersProducts = 1,
        BestSellersProductsByQuantity = 2,
        RelatedProducts = 3,
        MarkedAsNewProducts = 4,
        RecentlyViewedProducts = 5
    }
}
