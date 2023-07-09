using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.ColorVM
{
    public class ColorEditVM
    {
        [Required]
        public string Name { get; set; }
    }
}
