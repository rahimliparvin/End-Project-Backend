using System.ComponentModel.DataAnnotations;

namespace Savoy.Areas.Admin.ViewModels.CategoryVM
{
	public class CategoryEditVM
	{
		[Required]
		public string Name { get; set; }
	}
}
