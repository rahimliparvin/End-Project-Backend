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
        private readonly IProductService _productService;


        public HomeController(AppDbContext context,
                              ISliderService sliderService,
                              IBlogService blogService,
                              IProductService productService)
        {
            _context = context;
            _sliderService = sliderService;
            _blogService = blogService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Slider> sliders = await _sliderService.GetAllAsync();
            IEnumerable<Blog> blogs = await _blogService.GetAllAsync();
            IEnumerable<Product> products = await _productService.GetAllAsync();

            HomeVM model = new()
            {
                Sliders = sliders,
                Blogs = blogs,
                Products = products
            };

            return View(model);
        }
    }
}
