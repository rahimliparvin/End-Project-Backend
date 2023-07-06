using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.BlogVM
{
    public class BlogCreateVM
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public int TagId { get; set; }

        [Required]
        public List<IFormFile> Photos { get; set; }
    }
}
