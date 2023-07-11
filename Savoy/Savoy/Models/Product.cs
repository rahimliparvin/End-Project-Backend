namespace Savoy.Models
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int RatesCount { get; set; } = 1;
        public int RatesWorth { get; set; } = 5;
        public int SaleCount { get; set; }
        public int StockCount { get; set; }
        public string Sku { get; set; }
        public string Weight { get; set; }
        public string Dimensions { get; set; }
        public string Materials { get; set; }
        public string OtherInfo { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; }
        public ICollection<BasketItem> BasketItems { get; set; }
        public ICollection<ProductComment> ProductComments { get; set; }
        public ICollection<ProductTag> ProductTags { get; set; }
        public ICollection<ProductImage> ProductImages { get; set; }


        public Product()
        {
            BasketItems = new List<BasketItem>();
        }
    }
}
