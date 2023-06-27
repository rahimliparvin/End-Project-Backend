using Microsoft.AspNetCore.Mvc;

namespace Savoy.Controllers
{
    public class WishlistController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
