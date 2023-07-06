using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Savoy.Areas.Admin.ViewModels.BlogVM;
using Savoy.Areas.Admin.ViewModels.SliderVM;
using Savoy.Data;
using Savoy.Helpers;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Areas.Admin.Controllers
{
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
