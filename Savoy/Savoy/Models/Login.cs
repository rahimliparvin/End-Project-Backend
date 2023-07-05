using System.ComponentModel.DataAnnotations.Schema;

namespace Savoy.Models
{
    public class Login : BaseEntity
    {
        public string BackgroundImage { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }    
    }
}
