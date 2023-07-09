using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.LoginVM
{
    public class LoginEditVM
    {
      
        public string Image { get; set; }
        [Required]
        public IFormFile Photo { get; set; }
    }
}
