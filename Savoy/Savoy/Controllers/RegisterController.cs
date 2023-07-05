using Microsoft.AspNetCore.Mvc;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;

namespace Savoy.Controllers
{
    public class RegisterController : Controller
    {
        private readonly IRegisterService _registerService;

        public RegisterController(IRegisterService registerService)
        {
            _registerService = registerService;
        }

        public async Task<IActionResult> Index()
        {
            Register register = await _registerService.GetAsync();

            RegisterVM model = new()
            {
                Register = register
            };

            return View(model);
        }
    }
}
