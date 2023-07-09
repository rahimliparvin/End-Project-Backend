using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.TagVM
{
    public class TagEditVM
    {
        [Required]
        public string Name { get; set; }
    }
}
