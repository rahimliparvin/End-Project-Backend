using Microsoft.AspNetCore.Mvc;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;

namespace Savoy.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginService _loginService;

        public LoginController(ILoginService loginService)
        {
            _loginService = loginService;
        }

        public async Task<IActionResult> Index()
        {
            Login login = await _loginService.GetAsync();

            LoginVM model = new()
            {
                Login = login
            };

            return View(model);
        }
    }
}
