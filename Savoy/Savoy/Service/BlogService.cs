using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class BlogService : IBlogService
    {
        private readonly AppDbContext _context;
        public BlogService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Blog>> GetAllAsync() => await _context.Blogs.Include(m => m.Author).
                                                                                   Include(m => m.Images).
                                                                                   Include(m=>m.Category).
                                                                                   Include(m=>m.Tag).   
                                                                                   ToListAsync();


        public async Task<Blog> GetFullDataByIdAsync(int id) => await _context.Blogs.Include(m => m.Images).
                                                                                     Include(m => m.Author)?.
                                                                                     Include(m => m.Category).
                                                                                     Include(m => m.Tag).
                                                                                     FirstOrDefaultAsync(m => m.Id == id);
    }
}
