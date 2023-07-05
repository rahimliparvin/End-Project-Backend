using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;
using Savoy.ViewModels;

namespace Savoy.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IBlogService _blogService;
        private readonly ICategoryService _categoryService;

        public BlogController(AppDbContext context,
                              IBlogService blogService,
                              ICategoryService categoryService)
        {
            _context = context;
            _blogService = blogService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Blog> blogs = await _blogService.GetAllAsync();
            IEnumerable<Category> categories = await _context.Categories.Where(m => m.Blogs.Count > 0).ToListAsync();

            BlogVM model = new()
            {
                Blogs = blogs,
                Categories = categories
            };


            return View(model);
        }

        public async Task<IActionResult> Detail(int? id)
        {

            if (id == null) return BadRequest();

            Blog blog = await _blogService.GetFullDataByIdAsync((int)id);

            if (blog == null) return NotFound();

            BlogDetailVM blogDetailVM = new()
            {
                Title = blog.Title,
                Description = blog.Description,
                AuthorName = blog.Author.Name,
                TagName = blog.Tag.Name,
                CategoryName = blog.Category.Name,
                Images = blog.Images,
                CreateDate = blog.Created

            };


            return View(blogDetailVM);

        }



        [HttpGet]
        public async Task<IActionResult> GetCategoryBlogs(int? id)
        {
            if (id == null) return BadRequest();

            var blogs = await _context.Blogs.Where(m => m.Category.Id == id).
                                                            Include(m => m.Images).
                                                            ToListAsync();


            return PartialView("_BlogsPartial", blogs);
        }





        [HttpGet]
        public async Task<IActionResult> GetAllCategoriesBlogs()
        {
            var blogs = await _context.Blogs.Include(m => m.Images).ToListAsync();

            return PartialView("_BlogsPartial", blogs);
        }
    }
}
