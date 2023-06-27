using Microsoft.AspNetCore.Mvc;

namespace Savoy.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
