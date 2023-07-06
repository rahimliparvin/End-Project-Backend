using System.ComponentModel.DataAnnotations;

namespace ProniaBackEndProject.Areas.Admin.ViewModels
{
    public class SliderEditVM
    {
       
        [Required]
        public string Title {get;set; }
        [Required]
        public string CategoryName { get;set; }
        public string Image { get;set; }
        public IFormFile Photo { get; set; }
      
    }
}
