using Savoy.Models;

namespace Savoy.ViewModels
{
    public class ShopVM
    {
        public IEnumerable<Color> Colors { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
