using Savoy.Models;

namespace Savoy.ViewModels
{
    public class BlogDetailVM
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<BlogImage> Images { get; set; }
        public string AuthorName { get; set; }
        public string CategoryName { get; set; }
        public string TagName { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
