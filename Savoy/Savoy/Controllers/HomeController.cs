using Microsoft.AspNetCore.Mvc;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;

namespace Savoy.Controllers
{
    public class HomeController : Controller
    {

        private readonly AppDbContext _context;
        private readonly ISliderService _sliderService;
        private readonly IBlogService _blogService;


        public HomeController(AppDbContext context,
                              ISliderService sliderService,
                              IBlogService blogService)
        {
            _context = context;
            _sliderService = sliderService;
            _blogService = blogService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _sliderService.GetAllAsync();
            IEnumerable<Blog> blogs = await _blogService.GetAllAsync();

            HomeVM model = new()
            {
                Sliders = sliders,
                Blogs = blogs
            };

            return View(model);
        }
    }
}
