using Microsoft.AspNetCore.Mvc;

namespace Savoy.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
