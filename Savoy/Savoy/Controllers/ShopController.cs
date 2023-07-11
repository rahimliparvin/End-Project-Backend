using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Savoy.Areas.Admin.ViewModels.ProductVM;
using Savoy.Data;
using Savoy.Helpers;
using Savoy.Models;
using Savoy.Service;
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
        private readonly UserManager<AppUser> _userManager;

        public ShopController(AppDbContext context,
                              IColorService colorService,
                              ITagService tagService,
                              ICategoryService categoryService,
                              IProductService productService,
                              UserManager<AppUser> userManager)
        {
            _context = context;
            _colorService = colorService;
            _tagService = tagService;
            _categoryService = categoryService;
            _productService = productService;
            _userManager = userManager;
        }


        public async Task<IActionResult> Index(int? categoryId = null, int? tagId = null, int? colorId = null, int page = 1, int take = 8, string searchText = null)
        {

            IEnumerable<Color> colors = await _colorService.GetAllAsync();
            IEnumerable<Tag> tags = await _tagService.GetAllAsync();
            IEnumerable<Category> categories = await _categoryService.GetAllAsync();

            IEnumerable<Product> products = await _productService.GetPaginationData(categoryId, tagId, colorId, page, take, searchText);

            List<ShopVM> mappedDatas = GetMappedDatas(products, tags, colors, categories);

            int pageCount = await GetPageCountAsync(take, categoryId, searchText, tagId, colorId);

            Paginate<ShopVM> paginatedDatas = new(mappedDatas, page, pageCount);

            ViewBag.take = take;

            //ViewBag.searchText = searchText;


            return View(paginatedDatas);
        }

        private async Task<int> GetPageCountAsync(int take, int? categoryId, string searchText, int? tagId, int? colorId)
        {

            if (categoryId == null)
            {
                if (searchText != null)
                {

                    if (tagId != null && colorId != null)
                    {
                        int productSearchTextTagColor = await _context.Products.Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                  Trim().Contains(searchText.ToLower().Trim())).Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                  ThenInclude(m => m.Category).
                                                                                  Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                  Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                  CountAsync();

                        ViewBag.tagId = tagId;
                        ViewBag.colorId = colorId;
                        ViewBag.searchText = searchText;
                        return (int)Math.Ceiling((decimal)productSearchTextTagColor / take);
                    }
                    if (colorId != null && tagId == null)
                    {
                        int productSearchTextColor = await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                     ThenInclude(m => m.Category).
                                                                                     Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                     Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                     Trim().Contains(searchText.ToLower().Trim())).
                                                                                     CountAsync();


                        ViewBag.tagId = null;
                        ViewBag.colorId = colorId;
                        ViewBag.searchText = searchText;
                        return (int)Math.Ceiling((decimal)productSearchTextColor / take);
                    }
                    if (colorId == null && tagId != null)
                    {
                        int productSearchTextTag = await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                     ThenInclude(m => m.Category).
                                                                                     Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                     Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                     Trim().Contains(searchText.ToLower().Trim())).
                                                                                     CountAsync();

                        ViewBag.tagId = tagId;
                        ViewBag.colorId = null;
                        ViewBag.searchText = searchText;
                        return (int)Math.Ceiling((decimal)productSearchTextTag / take);

                    }




                    int productSearchTextCount = await _context.Products.Include(m => m.ProductImages).
                                                   Include(m => m.ProductCategories).
                                                   OrderByDescending(m => m.Id).
                                                   Where(m => !m.SoftDelete && m.Name.ToLower().
                                                   Trim().Contains(searchText.ToLower().Trim()))
                                                   .CountAsync();


                    ViewBag.tagId = null;
                    ViewBag.colorId = null;
                    ViewBag.searchText = searchText;

                    return (int)Math.Ceiling((decimal)productSearchTextCount / take);
                }

                if (tagId != null && colorId != null)
                {
                    int sortProductTagColor = await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                     ThenInclude(m => m.Category).
                                                                                     Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                     Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                     CountAsync();
                    ViewBag.searchText = null;
                    ViewBag.tagId = tagId;
                    ViewBag.colorId = colorId;
                    return (int)Math.Ceiling((decimal)sortProductTagColor / take);

                }

                if (tagId != null && colorId == null)
                {
                    int sortProductTag = await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                      ThenInclude(m => m.Category).
                                                                                      Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                      CountAsync();
                    ViewBag.searchText = null;
                    ViewBag.tagId = tagId;
                    ViewBag.colorId = null;
                    return (int)Math.Ceiling((decimal)sortProductTag / take);
                }

                if (tagId == null && colorId != null)
                {
                    int sortProductColor = await _context.Products.Include(m => m.ProductImages).Include(m => m.ProductCategories).
                                                                                      ThenInclude(m => m.Category).
                                                                                      Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                      CountAsync();
                    ViewBag.searchText = null;
                    ViewBag.tagId = null;
                    ViewBag.colorId = colorId;
                    return (int)Math.Ceiling((decimal)sortProductColor / take);

                }



                int producttCount = await _productService.GetCountAsync();
                ViewBag.categoryId = null;
                ViewBag.searchText = null;
                ViewBag.colorId = null;
                ViewBag.tagId = null;
                return (int)Math.Ceiling((decimal)producttCount / take);
            }

            if (searchText != null)
            {


                if (colorId != null && tagId != null)
                {
                    int productCategorySearchTextColorTag = await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                 Include(m => m.ProductImages).
                                                                                 Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                 Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                 Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).
                                                                                 CountAsync();



                    ViewBag.categoryId = categoryId;
                    ViewBag.searchText = searchText;
                    ViewBag.colorId = colorId;
                    ViewBag.tagId = tagId;
                    return (int)Math.Ceiling((decimal)productCategorySearchTextColorTag / take);
                }

                if (colorId != null && tagId == null)
                {
                    int productCategorySearchTextColor = await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                 Include(m => m.ProductImages).
                                                                                 Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                 Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).
                                                                                 CountAsync();

                    ViewBag.categoryId = categoryId;
                    ViewBag.searchText = searchText;
                    ViewBag.tagId = null;
                    ViewBag.colorId = colorId;
                    return (int)Math.Ceiling((decimal)productCategorySearchTextColor / take);
                }

                if (colorId == null && tagId != null)
                {
                    int productCategorySearchTextTag = await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                 Include(m => m.ProductImages).
                                                                                 Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                 Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                                 Trim().Contains(searchText.ToLower().Trim())).
                                                                                 CountAsync();


                    ViewBag.categoryId = categoryId;
                    ViewBag.searchText = searchText;
                    ViewBag.tagId = tagId;
                    ViewBag.colorId = null;
                    return (int)Math.Ceiling((decimal)productCategorySearchTextTag / take);
                }

                int productSearchTextCount = await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                      Include(m => m.ProductImages).
                                                                      Where(m => !m.SoftDelete && m.Name.ToLower().
                                                                      Trim().Contains(searchText.ToLower().Trim())).
                                                                      CountAsync();


                ViewBag.categoryId = categoryId;
                ViewBag.searchText = searchText;
                ViewBag.tagId = null;
                ViewBag.colorId = null;

                return (int)Math.Ceiling((decimal)productSearchTextCount / take);
            }

            if (tagId != null && colorId != null)
            {


                int sortProductCategoryTagColor = await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                     Include(m => m.ProductImages).
                                                                                     Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                     Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                                     CountAsync();

                ViewBag.tagId = tagId;
                ViewBag.colorId = colorId;
                ViewBag.categoryId = categoryId;
                ViewBag.searchText = null;
                return (int)Math.Ceiling((decimal)sortProductCategoryTagColor / take);

            }

            if (tagId != null && colorId == null)
            {


                int productCategoryTag = await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                             Include(m => m.ProductImages).
                                                                             Where(x => x.ProductTags.Any(xt => xt.TagId == tagId)).
                                                                             CountAsync();

                ViewBag.tagId = tagId;
                ViewBag.colorId = null;
                ViewBag.categoryId = categoryId;
                ViewBag.searchText = null;
                return (int)Math.Ceiling((decimal)productCategoryTag / take);
            }

            if (tagId == null && colorId != null)
            {
                int productCategoryColor = await _context.Products.Where(x => x.ProductCategories.Any(xt => xt.CategoryId == categoryId)).
                                                                                                  Include(m => m.ProductImages).
                                                                                                  Where(x => x.ProductColors.Any(xt => xt.ColorId == colorId)).
                                                                                                  CountAsync();
                ViewBag.tagId = null;
                ViewBag.colorId = colorId;
                ViewBag.categoryId = categoryId;
                ViewBag.searchText = null;
                return (int)Math.Ceiling((decimal)productCategoryColor / take);

            }


            int producttCounts = await _productService.GetCountAsync();
            ViewBag.ProductCount = producttCounts;

            ViewBag.categoryId = categoryId;
            ViewBag.colorId = null;
            ViewBag.tagId = null;
            ViewBag.searchText = null;

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
                Id = product.Id,
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




        public async Task<IActionResult> AddBasket(int id, int quantity)
        {

            Product product = await _productService.GetFullDataByIdAsync((int)id);

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.ProductId == product.Id && b.AppUserId == user.Id);
                if (basketItem == null)
                {
                    if (product.StockCount < 1)
                    {
                        return Redirect(Request.Headers["Referer"].ToString());
                    }

                    if(product.StockCount < quantity)
                    {
                        return Redirect(Request.Headers["Referer"].ToString());
                    }

                    basketItem = new BasketItem()
                    {
                        AppUserId = user.Id,
                        ProductId = product.Id,
                        Count = quantity
                    };
                    _context.BasketItems.Add(basketItem);
                  
                }
                else
                {
                    if (product.StockCount < 1)
                    {
                        return Redirect(Request.Headers["Referer"].ToString());
                    }

                    basketItem.Count = basketItem.Count + quantity;
                }

                product.StockCount = product.StockCount - quantity;
                product.SaleCount = product.SaleCount + quantity;
                _context.SaveChanges();
                return Redirect(Request.Headers["Referer"].ToString());
            }

            return RedirectToAction("Login","Account");
        }

        public async Task<IActionResult> RemoveBasketItem(int id)
        {
            Product product = await _productService.GetFullDataByIdAsync((int)id);

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(b => b.ProductId == product.Id && b.AppUserId == user.Id);
                if (basketItem != null)
                {
                    product.StockCount = product.StockCount + basketItem.Count;

                    _context.BasketItems.Remove(basketItem);
                    _context.SaveChanges();
                }
            }

            return Redirect(Request.Headers["Referer"].ToString());
        }

        public async Task<IActionResult> BasketProductCountChange(int basketId, int quantity)
        {
            if (basketId < 1) return NotFound();
            BasketItem item = _context.BasketItems.FirstOrDefault(x => x.Id == basketId);
            if (item is null) return NotFound();

            item.Count = item.Count + quantity;

            if (item.Count < 0) return RedirectToAction("Index", "Cart");

            Product product = await _productService.GetFullDataByIdAsync(item.ProductId);

            product.StockCount = product.StockCount - quantity;
            product.SaleCount = product.SaleCount + quantity;

            if (product.StockCount < 0) return RedirectToAction("Index", "Cart");
            _context.SaveChanges();
            //item.Count= ++quantity;
            return RedirectToAction("Index", "Cart");
        }



        public async Task<IActionResult> AddWishlist(int productId)
        {

            Product product = await _productService.GetFullDataByIdAsync((int)productId);

            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                WishlistItem wishlistItem = _context.WishlistItems.FirstOrDefault(b => b.ProductId == product.Id && b.AppUserId == user.Id);

                if(wishlistItem == null)
                {

                    wishlistItem = new WishlistItem()
                    {
                        AppUserId = user.Id,
                        ProductId = product.Id,

                    };
                    _context.WishlistItems.Add(wishlistItem);
                    _context.SaveChanges();
                }
                else
                {
                    _context.WishlistItems.Remove(wishlistItem);
                    _context.SaveChanges();
                }


                return Redirect(Request.Headers["Referer"].ToString());
            }

            return RedirectToAction("Login","Account");
        }


        //public async Task<IActionResult> RemoveWishlist(int id)
        //{
        //    Product product = await _productService.GetFullDataByIdAsync((int)id);

        //    if (User.Identity.IsAuthenticated)
        //    {
        //        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        WishlistItem wishlistItem = _context.WishlistItems.FirstOrDefault(b => b.ProductId == product.Id && b.AppUserId == user.Id);
        //        if (wishlistItem != null)
        //        {
                    

        //            _context.WishlistItems.Remove(wishlistItem);
        //            _context.SaveChanges();
        //        }
        //    }

        //    return Redirect(Request.Headers["Referer"].ToString());
        //}










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
