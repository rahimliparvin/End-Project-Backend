namespace Savoy.Areas.Admin.ViewModels.ProductVM
{
    public class ProductDetailsVM
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
        public IEnumerable<string> Images { get; set; }
        public IEnumerable<string> Colors { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
