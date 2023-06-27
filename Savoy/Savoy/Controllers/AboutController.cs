using Microsoft.AspNetCore.Mvc;

namespace Savoy.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
