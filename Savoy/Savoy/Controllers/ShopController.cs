using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;
using System.Collections.Immutable;

namespace Savoy.Controllers
{
    public class ShopController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IColorService _colorService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public ShopController(AppDbContext context,
                              IColorService colorService, 
                              ITagService tagService,
                              ICategoryService categoryService,
                              IProductService productService)
        {
            _context = context;
            _colorService = colorService;
            _tagService = tagService;
            _categoryService = categoryService;
            _productService = productService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            IEnumerable<Color> colors = await _colorService.GetAllAsync();
            IEnumerable<Tag> tags = await _tagService.GetAllAsync();
            IEnumerable<Category> categories = await _categoryService.GetAllAsync();
            IEnumerable<Product> products = await _productService.GetAllAsync();
            if(id != null)
            {
                products = products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == id)).ToList();
            }
            else
            {
                products = await _productService.GetAllAsync();
            }
            ShopVM model = new()
            {
                Colors = colors,
                Tags = tags,
                Categories = categories,
                Products = products
            };

            return View(model);
        }


        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return BadRequest();

            Product product = await _productService.GetFullDataByIdAsync((int)id);

            if (product == null) return NotFound();

            List<string> images = new List<string>();

            foreach (var image in product.ProductImages)
            {
                images.Add(image.Image);
            }


            List<string> tags = new List<string>();

            foreach (var tag in product.ProductTags.Select(pt => pt.Tag))
            {
                tags.Add(tag.Name);
            }

            List<string> colors = new List<string>();

            foreach (var color in product.ProductColors.Select(pt => pt.Color))
            {
                colors.Add(color.Name);
            }

            List<string> categories = new List<string>();

            foreach (var category in product.ProductCategories.Select(pt => pt.Category))
            {
                categories.Add(category.Name);
            }

            ShopDetailVM shopDetailVM = new()
            {
                Name = product.Name,
                Description = product.Description,
                StockCount = product.StockCount,
                SaleCount = product.SaleCount,
                Price = product.Price,
                RatesCount = product.RatesCount,
                RatesWorth = product.RatesWorth,
                Sku = product.Sku,
                Weight = product.Weight,
                Dimensions = product.Dimensions,
                Materials = product.Materials,
                OtherInfo = product.OtherInfo,
                ProductImages = images,
                ProductCategories = categories,
                ProductTags = tags,
                ProductColors = colors

            };

            return View(shopDetailVM);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryProducts(int? id)
        {
            if (id == null) return BadRequest();

            var products = await _context.ProductCategories.Where(m => m.Category.Id == id).
                                                            Include(m => m.Product).
                                                            ThenInclude(m => m.ProductImages).
                                                            Select(m => m.Product).
                                                            ToListAsync();


            return PartialView("_ProductPartial", products);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesProducts()
        {
            var products = await _context.Products.Include(m => m.ProductImages).ToListAsync();



            return PartialView("_ProductPartial", products);
        }
    }
}
