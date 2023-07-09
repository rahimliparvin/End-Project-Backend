using Savoy.Models;
using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.ProductVM
{
    public class ProductEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Price { get; set; }
        [Required]
        public int RatesCount { get; set; } = 1;
        [Required]
        public int RatesWorth { get; set; } = 5;
        [Required]
        public int SaleCount { get; set; }
        [Required]
        public int StockCount { get; set; }
        [Required]
        public string Sku { get; set; }
        [Required]
        public string Weight { get; set; }
        [Required]
        public string Dimensions { get; set; }
        [Required]
        public string Materials { get; set; }
        [Required]
        public string OtherInfo { get; set; }
        public ICollection<ProductImage> Images { get; set; }
        public List<IFormFile> Photos { get; set; }
        [Required]
        public List<int> ProductColorsIds { get; set; } = new();
        [Required]
        public List<int> ProductCategoriesIds { get; set; } = new();
        [Required]
        public List<int> ProductTagsIds { get; set; } = new();
    }
}

