using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.RegisterVM
{
    public class RegisterEditVM
    {
        public string Image { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
