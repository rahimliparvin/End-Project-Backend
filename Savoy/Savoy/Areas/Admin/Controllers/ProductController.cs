using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Savoy.Areas.Admin.ViewModels.ProductVM;
using Savoy.Data;
using Savoy.Helpers;
using Savoy.Models;
using Savoy.Service;
using Savoy.Service.Interfaces;
using System.Data;

namespace Savoy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly AppDbContext _context;
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IColorService _colorService;
        private readonly ITagService _tagService;

        public ProductController(IWebHostEnvironment env,
                                 AppDbContext context,
                                 IProductService productService,
                                 ICategoryService categoryService,
                                 IColorService colorService,
                                 ITagService tagService)
        {
            _env = env;
            _context = context;
            _productService = productService;
            _colorService = colorService;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int page = 1, int take = 5)
        {
            IEnumerable<Product> products = await _productService.GetPaginationDatas(page, take);

            IEnumerable<ProductIndexVM> mappedDatas = GetMappedDatas(products);

            int pageCount = await GetPageCountAsync(take);

            Paginate<ProductIndexVM> paginatedDatas = new(mappedDatas, page, pageCount);

            ViewBag.take = take;

            return View(paginatedDatas);
        }

        private async Task<int> GetPageCountAsync(int take)
        {
            var productCount = await _productService.GetCountAsync();
            return (int)Math.Ceiling((decimal)productCount / take);
        }


        private IEnumerable<ProductIndexVM> GetMappedDatas(IEnumerable<Product> products)
        {
            List<ProductIndexVM> mappedDatas = new();

            foreach (var product in products)
            {
                ProductIndexVM prodcutVM = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    CategoryName = product.ProductCategories.Select(m => m.Category)?.FirstOrDefault()?.Name,
                    MainImage = product.ProductImages.Where(m => m.IsMain).FirstOrDefault()?.Image
                };
                mappedDatas.Add(prodcutVM);
            }

            return mappedDatas;
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.colors = await GetColorsAsync();
            ViewBag.tags = await GetTagsAsync();

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM model)
        {


            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.colors = await GetColorsAsync();
            ViewBag.tags = await GetTagsAsync();


            if (!ModelState.IsValid)
            {
                return View(model);
            }



            IEnumerable<Product> products = await _productService.GetAllAsync();


            List<ProductColor> colors = new();

            foreach (var colorId in model.ProductColorsIds)
            {
                ProductColor color = new()
                {
                    ColorId = colorId
                };

                colors.Add(color);
            }

            List<ProductTag> tags = new();

            foreach (var tagId in model.ProductTagsIds)
            {
                ProductTag tag = new()
                {
                    TagId = tagId
                };

                tags.Add(tag);
            }

            List<ProductCategory> categories = new();

            foreach (var categoryId in model.ProductCategoriesIds)
            {
                ProductCategory category = new()
                {
                    CategoryId = categoryId
                };

                categories.Add(category);
            }




            foreach (var photo in model.Photos)
            {
                if (!photo.CheckFileType("image/"))
                {

                    ModelState.AddModelError("Photos", "File type must be image");
                    return View();

                }


                if (photo.CheckFileSize(1000))
                {

                    ModelState.AddModelError("Photos", "Photo size must be max 1000Kb");
                    return View();

                }

            }


            List<ProductImage> productImages = new();


            foreach (var photo in model.Photos)
            {

                string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);

                await FileHelper.SaveFileAsync(path, photo);

                ProductImage productImage = new()
                {
                    Image = fileName
                };

                productImages.Add(productImage);
            }


            productImages.FirstOrDefault().IsMain = true;

            if(productImages.Count > 1)
            {
                productImages.Skip(1).FirstOrDefault().HoverImage = true;
            }
            else
            {
                productImages.FirstOrDefault().HoverImage = true;

            }
          


            Product newProduct = new()
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                Sku = model.Sku,
                OtherInfo = model.OtherInfo,
                RatesCount = model.RatesCount,
                RatesWorth =  model.RatesWorth,
                SaleCount = model.SaleCount,
                StockCount = model.StockCount,
                Weight = model.Weight,
                Materials = model.Materials,
                Dimensions = model.Dimensions,
                ProductCategories = categories,
                ProductColors = colors,
                ProductTags = tags,
                ProductImages = productImages,


            };

            await _context.ProductImages.AddRangeAsync(productImages);
            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));


        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {

            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.colors = await GetColorsAsync();
            ViewBag.tags = await GetTagsAsync();

            if (id == null) return BadRequest();

            Product product = await _productService.GetFullDataByIdAsync(id);

            if (product == null) return NotFound();

            ProductEditVM model = new()
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price.ToString(),
                SaleCount = product.SaleCount,
                StockCount = product.StockCount,
                RatesCount = product.RatesCount,
                RatesWorth = product.RatesWorth,
                Materials = product.Materials,
                Weight = product.Weight,
                Dimensions = product.Dimensions,
                Sku = product.Sku,
                OtherInfo = product.OtherInfo,
                Images = product.ProductImages,
                ProductCategoriesIds = product.ProductCategories.Select(m => m.CategoryId).ToList(),
                ProductTagsIds = product.ProductTags.Select(m => m.TagId).ToList(),
                ProductColorsIds = product.ProductColors.Select(m => m.ColorId).ToList()

            };


            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, ProductEditVM model)
        {
            if (id == null) return BadRequest();

            Product dbProduct = await _productService.GetFullDataByIdAsync((int)id);

            if (dbProduct == null) return NotFound();

            ViewBag.categories = await GetCategoriesAsync();
            ViewBag.colors = await GetColorsAsync();
            ViewBag.tags = await GetTagsAsync();


            if (!ModelState.IsValid)
            {
                return View(model);
            }


            List<ProductCategory> categories = new();

            foreach (var categoryId in model.ProductCategoriesIds)
            {
                ProductCategory category = new()
                {
                    CategoryId = categoryId
                };

                categories.Add(category);
            }

            List<ProductColor> colors = new();

            foreach (var colorId in model.ProductColorsIds)
            {
                ProductColor color = new()
                {
                    ColorId = colorId
                };

                colors.Add(color);
            }


            List<ProductTag> tags = new();

            foreach (var tagId in model.ProductTagsIds)
            {
                ProductTag tag = new()
                {
                    TagId = tagId
                };

                tags.Add(tag);
            }


            List<ProductImage> productImages = new();

            if (model.Photos != null)
            {

                foreach (var item in model.Photos)
                {

                    if (!item.CheckFileType("image/"))
                    {

                        ModelState.AddModelError("Photos", "File type must be image");
                        return View();

                    }


                    if (item.CheckFileSize(1000))
                    {

                        ModelState.AddModelError("Photos", "Photo size must be max 1000Kb");
                        return View();

                    }

                    string fileName = Guid.NewGuid().ToString() + "_" + item.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);

                    await FileHelper.SaveFileAsync(path, item);

                    ProductImage productImage = new()
                    {
                        Image = fileName
                    };

                    productImages.Add(productImage);


                }

                foreach (var item in dbProduct.ProductImages)
                {

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", item.Image);

                    FileHelper.DeleteFile(path);
                }


                productImages.FirstOrDefault().IsMain = true;
                productImages.Skip(1).FirstOrDefault().HoverImage = true;

                dbProduct.ProductImages = productImages;

            }
            else
            {
                dbProduct.ProductImages = dbProduct.ProductImages;
            }



            decimal convertedPrice = decimal.Parse(model.Price);

            dbProduct.Name = model.Name;
            dbProduct.Description = model.Description;
            dbProduct.Price = convertedPrice;
            dbProduct.Sku = model.Sku;
            dbProduct.RatesCount = dbProduct.RatesCount;
            dbProduct.RatesWorth = dbProduct.RatesWorth;
            dbProduct.OtherInfo = model.OtherInfo;
            dbProduct.SaleCount = model.SaleCount;
            dbProduct.StockCount = model.StockCount;
            dbProduct.Dimensions = model.Dimensions;
            dbProduct.Materials = model.Materials;
            dbProduct.Weight = model.Weight;
            dbProduct.ProductCategories = categories;
            dbProduct.ProductColors = colors;
            dbProduct.ProductTags = tags;
            dbProduct.Updated = DateTime.Now;




            await _context.ProductImages.AddRangeAsync(productImages);
            _context.Products.Update(dbProduct);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Product product = await _productService.GetFullDataByIdAsync((int)id);

            if (product is null) return NotFound();


            foreach (var item in product.ProductImages)
            {
                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", item.Image);
                FileHelper.DeleteFile(path);

            }


            _context.Products.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            Product product = await _productService.GetFullDataByIdAsync((int)id);
            if (product is null) return NotFound();



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


            List<string> categories = new List<string>();

            foreach (var category in product.ProductCategories.Select(pt => pt.Category))
            {
                categories.Add(category.Name);
            }


            List<string> colors = new List<string>();

            foreach (var color in product.ProductColors.Select(pt => pt.Color))
            {
                colors.Add(color.Name);
            }

            ProductDetailsVM model = new ProductDetailsVM
            {
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                SaleCount = product.SaleCount,
                StockCount = product.StockCount,
                Sku = product.Sku,
                RatesCount = product.RatesCount,
                RatesWorth = product.RatesWorth,
                Weight = product.Weight,
                Dimensions = product.Dimensions,
                Materials = product.Materials,
                OtherInfo = product.OtherInfo,
                Images = images,
                Categories = categories,
                Colors = colors,
                Tags = tags,
                CreatedAt = product.Created,
                UpdatedAt = product.Updated
            };

            return View(model);
        }



        private async Task<SelectList> GetColorsAsync()
        {

            IEnumerable<Color> colors = await _colorService.GetAllAsync();

            return new SelectList(colors, "Id", "Name");


        }

        private async Task<SelectList> GetCategoriesAsync()
        {

            IEnumerable<Category> categories = await _categoryService.GetAllAsync();

            return new SelectList(categories, "Id", "Name");


        }

        private async Task<SelectList> GetTagsAsync()
        {

            IEnumerable<Tag> tags = await _tagService.GetAllAsync();

            return new SelectList(tags, "Id", "Name");


        }


    }
}
