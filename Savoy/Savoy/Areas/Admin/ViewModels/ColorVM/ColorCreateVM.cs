using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.ColorVM
{
    public class ColorCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
