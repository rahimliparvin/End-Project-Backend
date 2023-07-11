using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Savoy.Helpers;

namespace Savoy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
