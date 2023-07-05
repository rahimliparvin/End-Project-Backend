using Savoy.Models;

namespace Savoy.ViewModels
{
    public class BlogVM
    {
        public IEnumerable<Blog> Blogs { get; set; }
        public IEnumerable<Category> Categories { get; set; }

    }
}
