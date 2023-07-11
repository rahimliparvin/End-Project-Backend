using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Savoy.Areas.Admin.ViewModels.BlogVM;
using Savoy.Areas.Admin.ViewModels.SliderVM;
using Savoy.Data;
using Savoy.Helpers;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;
using System.Data;

namespace Savoy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IBlogService _blogService;
        private readonly IAuthorService _authorService;
        private readonly ICategoryService _categoryService;
        private readonly ITagService _tagService;

        public BlogController(AppDbContext context,
                              IWebHostEnvironment env,
                              IBlogService blogService,
                              IAuthorService authorService,
                              ICategoryService categoryService,
                              ITagService tagService)
        {
            _context = context;
            _env = env;
            _blogService = blogService;
            _authorService = authorService;
            _categoryService = categoryService;
            _tagService = tagService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blogs = await _blogService.GetAllAsync();

            List<BlogIndexVM> mappedDatas = new();

            foreach (var blog in blogs)
            {
                BlogIndexVM blogVM = new()
                {
                    Id = blog.Id,
                    Title = blog.Title,
                    Author = blog.Author.Name,
                    Image = blog.Images.FirstOrDefault()?.Image
                };

                mappedDatas.Add(blogVM);

            }

            return View(mappedDatas);
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {

            ViewBag.author = await GetAuthorsAsync();
            ViewBag.category = await GetCategoriesAsync();
            ViewBag.tag = await GetTagsAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BlogCreateVM model)
        {
            try
            {


                ViewBag.author = await GetAuthorsAsync();
                ViewBag.category = await GetCategoriesAsync();
                ViewBag.tag = await GetTagsAsync();

                if (!ModelState.IsValid)
                {
                    return View(model);
                }


                foreach (var photo in model.Photos)
                {

                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photos", "File type must be image");
                        return View();
                    }

                    if (photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photos", "Image size must be max 200kb");
                        return View();
                    }



                }

                List<BlogImage> blogImages = new();

                foreach (var photo in model.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);

                    await FileHelper.SaveFileAsync(path, photo);


                    BlogImage blogImage = new()
                    {
                        Image = fileName
                    };

                    blogImages.Add(blogImage);

                }

                blogImages.FirstOrDefault().IsMain = true;



                Blog newBlog = new()
                {
                    Title = model.Title,
                    Description = model.Description,
                    AuthorId = model.AuthorId,
                    CategoryId = model.CategoryId,
                    TagId = model.TagId,
                    Images = blogImages
                };

                await _context.BlogImages.AddRangeAsync(blogImages);
                await _context.Blogs.AddAsync(newBlog);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {

                throw;
            }
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            Blog dbProduct = await _blogService.GetFullDataByIdAsync((int)id);

            if (dbProduct is null) return NotFound();

            ViewBag.author = await GetAuthorsAsync();
            ViewBag.category = await GetCategoriesAsync();
            ViewBag.tag = await GetTagsAsync();




            return View(new BlogEditVM
            {
                Title = dbProduct.Title,
                Description = dbProduct.Description,
                AuthorId = dbProduct.AuthorId,
                CategoryId = dbProduct.CategoryId,
                TagId = dbProduct.TagId,
                Images = dbProduct.Images
            });


        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, BlogEditVM model)
        {
            ViewBag.author = await GetAuthorsAsync();
            ViewBag.category = await GetCategoriesAsync();
            ViewBag.tag = await GetTagsAsync();

            if (!ModelState.IsValid) return View(model);

            Blog dbBlog = await _blogService.GetFullDataByIdAsync((int)id);

            if (dbBlog is null) return NotFound();

            if (model.Photos != null)
            {

                foreach (var photo in model.Photos)
                {

                    if (!photo.CheckFileType("image/"))
                    {
                        ModelState.AddModelError("Photos", "File type must be image");
                        return View(dbBlog);
                    }

                    if (photo.CheckFileSize(200))
                    {
                        ModelState.AddModelError("Photos", "Image size must be max 200kb");
                        return View(dbBlog);
                    }



                }

                foreach (var item in dbBlog.Images)
                {

                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", item.Image);

                    FileHelper.DeleteFile(path);
                }



                List<BlogImage> productImages = new();

                foreach (var photo in model.Photos)
                {
                    string fileName = Guid.NewGuid().ToString() + "_" + photo.FileName;


                    string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", fileName);

                    await FileHelper.SaveFileAsync(path, photo);


                    BlogImage productImage = new()
                    {
                        Image = fileName
                    };

                    productImages.Add(productImage);

                }

                productImages.FirstOrDefault().IsMain = true;

                dbBlog.Images = productImages;
            }



            dbBlog.Title = model.Title;
            dbBlog.Description = model.Description;
            dbBlog.AuthorId = model.AuthorId;
            dbBlog.CategoryId = model.CategoryId;
            dbBlog.TagId = model.TagId;

            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        public async Task<IActionResult> Detail(int? id)
        {
            if (id is null) return BadRequest();

            //ViewBag.author = await GetAuthorsAsync();

            Blog dbBlog = await _blogService.GetFullDataByIdAsync((int)id);

            //ViewBag.desc = Regex.Replace(dbBlog.Description, "<.*?>", String.Empty);

            return View(new ViewModels.BlogVM.BlogDetailVM
            {

                Title = dbBlog.Title,
                Description = dbBlog.Description,
                AuthorId = dbBlog.AuthorId,
                Images = dbBlog.Images,
                AuthorName = dbBlog.Author.Name,
                CategoryId = dbBlog.CategoryId,
                CategoryName = dbBlog.Category.Name,
                TagId = dbBlog.TagId,
                TagName = dbBlog.Tag.Name
            });
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            Blog product = await _blogService.GetFullDataByIdAsync((int)id);

            if (product is null) return NotFound();


            foreach (var item in product.Images)
            {

                string path = FileHelper.GetFilePath(_env.WebRootPath, "assets/image", item.Image);

                FileHelper.DeleteFile(path);

            }


            _context.Blogs.Remove(product);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }


        private async Task<SelectList> GetAuthorsAsync()
        {

            IEnumerable<Author> authors = await _authorService.GetAllAsync();

            return new SelectList(authors, "Id", "Name");


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
