using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.TeamVM
{
    public class TeamEditVM
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Position { get; set; }
        public string Image { get; set; }
        public IFormFile Photo { get; set; }
    }
}
