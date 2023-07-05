using System.ComponentModel.DataAnnotations.Schema;

namespace Savoy.Models
{
    public class Slider : BaseEntity
    {
        public string Title { get; set; }
        public string CategoryName { get; set; }
        public string Image { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
