using Savoy.Models;
using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.BlogVM
{
    public class BlogEditVM
    {

        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int TagId { get; set; }
        public string TagName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<BlogImage> Images { get; set; }
        public List<IFormFile> Photos { get; set; }
    }
}
