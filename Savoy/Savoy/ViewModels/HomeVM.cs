using Savoy.Models;

namespace Savoy.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> Sliders { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
