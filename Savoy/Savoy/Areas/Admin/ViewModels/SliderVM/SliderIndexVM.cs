using System.ComponentModel.DataAnnotations.Schema;

namespace Savoy.Areas.Admin.ViewModels.SliderVM
{
	public class SliderIndexVM
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string CategoryName { get; set; }
		public string Image { get; set; }
		public bool SoftDelete { get; set; }
	}
}
