using Microsoft.AspNetCore.Mvc;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;

namespace Savoy.Controllers
{
    public class AboutController : Controller
    {
        private readonly ITeamService _teamService;
        private readonly IAboutService _aboutService;

        public AboutController(ITeamService teamService,    
                               IAboutService aboutService)
        {
            _teamService = teamService;
            _aboutService = aboutService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Team> teams = await _teamService.GetAllAsync();
            About about = await _aboutService.GetAllAsync();

            AboutVM model = new()
            {
                Teams = teams,
                About = about
            };

            return View(model);
        }
    }
}
