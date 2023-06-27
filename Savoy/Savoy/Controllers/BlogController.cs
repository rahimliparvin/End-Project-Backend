using Microsoft.AspNetCore.Mvc;

namespace Savoy.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Detail(int id)
        {

            if (id == null) return BadRequest();

            //Product product = await _productService.GetFullDataByIdAsync(id);

            //if (product == null) return NotFound();

            return View();

        }
    }
}
