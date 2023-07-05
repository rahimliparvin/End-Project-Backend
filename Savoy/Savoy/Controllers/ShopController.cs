using Microsoft.AspNetCore.Mvc;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;

namespace Savoy.Controllers
{
    public class ShopController : Controller
    {
        private readonly IColorService _colorService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;

        public ShopController(IColorService colorService, 
                              ITagService tagService,
                              ICategoryService categoryService)
        {
            _colorService = colorService;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Color> colors = await _colorService.GetAllAsync();
            IEnumerable<Tag> tags = await _tagService.GetAllAsync();
            IEnumerable<Category> categories = await _categoryService.GetAllAsync();

            ShopVM model = new()
            {
                Colors = colors,
                Tags = tags,
                Categories = categories
            };

            return View(model);
        }


        public async Task<IActionResult> Detail(int id)
        {

            return View();
        }
    }
}
