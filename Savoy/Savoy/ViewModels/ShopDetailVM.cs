using Savoy.Models;

namespace Savoy.ViewModels
{
    public class ShopDetailVM
    {
        public int Id { get; set; }
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
        public ICollection<string> ProductColors { get; set; }
        public ICollection<int> ColorsId { get; set; }

        public ICollection<string> ProductCategories { get; set; }
        public ICollection<string> ProductTags { get; set; }
        public ICollection<string> ProductImages { get; set; }

    }
}
