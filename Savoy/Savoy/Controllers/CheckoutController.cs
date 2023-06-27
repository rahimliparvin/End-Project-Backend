using Microsoft.AspNetCore.Mvc;

namespace Savoy.Controllers
{
    public class CheckoutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
