using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Savoy.Areas.Admin.ViewModels.ProductVM;
using Savoy.Data;
using Savoy.Helpers;
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

        //public async Task<IActionResult> Index(int? id)
        //{
        //    IEnumerable<Color> colors = await _colorService.GetAllAsync();
        //    IEnumerable<Tag> tags = await _tagService.GetAllAsync();
        //    IEnumerable<Category> categories = await _categoryService.GetAllAsync();
        //    IEnumerable<Product> products = await _productService.GetAllAsync();
        //    if(id != null)
        //    {
        //        products = products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == id)).ToList();
        //    }
        //    else
        //    {
        //        products = await _productService.GetAllAsync();
        //    }
        //    ShopVM model = new()
        //    {
        //        Colors = colors,
        //        Tags = tags,
        //        Categories = categories,
        //        Products = products
        //    };

        //    return View(model);
        //}

        public async Task<IActionResult> Index(int? categoryId = null, int? tagId = null, int? colorId = null, int page = 1, int take = 5, string searchText = null)
        {

            IEnumerable<Color> colors = await _colorService.GetAllAsync();
            IEnumerable<Tag> tags = await _tagService.GetAllAsync();
            IEnumerable<Category> categories = await _categoryService.GetAllAsync();

            IEnumerable<Product> products = await _productService.GetPaginationData(categoryId, tagId, colorId, page, take, searchText);

            List<ShopVM> mappedDatas = GetMappedDatas(products,tags,colors,categories);

            int pageCount = await GetPageCountAsync(take,categoryId,searchText);

            Paginate<ShopVM> paginatedDatas = new(mappedDatas, page, pageCount);

            ViewBag.take = take;

            ViewBag.searchText = searchText;

            //return PartialView("_ProductPartial", paginatedDatas);

            return View(paginatedDatas);
        }

        private async Task<int> GetPageCountAsync(int take,int? categoryId,string searchText)
        {
            if(categoryId == null)
            {
                if(searchText != null)
                {
                    int productSearchTextCount = await  _context.Products.Include(m => m.ProductImages).
                                                   Include(m => m.ProductCategories).
                                                   OrderByDescending(m => m.Id).
                                                   Where(m => !m.SoftDelete && m.Name.ToLower().
                                                   Trim().Contains(searchText.ToLower().Trim()))
                                                   .CountAsync();

                    return (int)Math.Ceiling((decimal)productSearchTextCount / take);
                }
                int producttCount = await _productService.GetCountAsync();
                ViewBag.categoryId = null;
                return (int)Math.Ceiling((decimal)producttCount / take);
            }

            int producttCounts = await _productService.GetCountAsync();
            ViewBag.ProductCount = producttCounts;

            ViewBag.categoryId = categoryId;

            var productCount = await _productService.GetCategoryIdProductCountAsync(categoryId);
            return (int)Math.Ceiling((decimal)productCount / take);
        }


        private List<ShopVM> GetMappedDatas(IEnumerable<Product> products,
                                            IEnumerable<Tag> tags,
                                            IEnumerable<Color> colors,
                                            IEnumerable<Category> categories)
        {

          


            List<ShopVM> mappedDatas = new();

            ShopVM prodcutVM = new()
            {
                Products = products,
                Categories = categories,
                Tags = tags,
                Colors = colors
            };
            mappedDatas.Add(prodcutVM);


            return mappedDatas;
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


        public async Task<IActionResult> MainSearch(string searchText)
        {
            var products = await _context.Products.Include(m => m.ProductImages).
                                                   Include(m => m.ProductCategories).
                                                   OrderByDescending(m => m.Id).
                                                   Where(m => !m.SoftDelete && m.Name.ToLower().
                                                   Trim().Contains(searchText.ToLower().Trim()))
                                                   .Take(10).ToListAsync();

            return PartialView("_ProductPartial", products);
        }
    }
}
