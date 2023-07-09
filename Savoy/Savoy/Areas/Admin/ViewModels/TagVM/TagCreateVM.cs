

using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.TagVM
{
    public class TagCreateVM
    {
        [Required]
        public string Name { get; set; }
    }
}
