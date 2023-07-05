using System.ComponentModel.DataAnnotations.Schema;

namespace Savoy.Models
{
    public class Team : BaseEntity
    {
        public string Name { get; set; }
        public string Position { get; set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }
    }
}
