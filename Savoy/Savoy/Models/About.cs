using System.ComponentModel.DataAnnotations.Schema;

namespace Savoy.Models
{
    public class About : BaseEntity
    {
        public string Description { get; set; }
        public string Phone { get; set; }
        public string Website { get; set; }
        public string Address { get;set; }
        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }  
    }
}
