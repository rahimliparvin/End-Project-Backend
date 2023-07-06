using System.ComponentModel.DataAnnotations;

namespace ProniaBackEndProject.Areas.Admin.ViewModels.AuthorVM
{
    public class AuthorEditVM
    {
        [Required]
        public string Name { get; set; }
    }
}
