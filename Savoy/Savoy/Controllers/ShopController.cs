using Microsoft.AspNetCore.Mvc;

namespace Savoy.Controllers
{
    public class ShopController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        public async Task<IActionResult> Detail(int id)
        {

            return View();
        }
    }
}
