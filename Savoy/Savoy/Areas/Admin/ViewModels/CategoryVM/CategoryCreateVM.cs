using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.CategoryVM
{
	public class CategoryCreateVM
	{
		[Required]
		public string Name { get; set; }
	}
}
