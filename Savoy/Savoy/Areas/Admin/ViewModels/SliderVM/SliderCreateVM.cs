using System.ComponentModel.DataAnnotations;

namespace ProniaBackEndProject.Areas.Admin.ViewModels
{
	public class SliderCreateVM
	{

        [Required]
        public string Title { get; set; }
        [Required]
        public string CategoryName { get; set; }
        [Required]
        public IFormFile Photo { get; set; }

	}
}
