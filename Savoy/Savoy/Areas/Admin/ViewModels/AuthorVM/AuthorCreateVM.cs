using System.ComponentModel.DataAnnotations;

namespace ProniaBackEndProject.Areas.Admin.ViewModels.AuthorVM
{
    public class AuthorCreateVM
    {
        [Required]
        public string Name { get; set; }    
    }
}
