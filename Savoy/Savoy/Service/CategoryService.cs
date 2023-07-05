using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly AppDbContext _context;
        public CategoryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync() => await _context.Categories.Include(m=>m.Blogs).    
                                                                                            Include(m => m.ProductCategories).
                                                                                            ThenInclude(m => m.Product)?.
                                                                                            ToListAsync();

        public async Task<Category> GetByIdAsync(int id) => await _context.Categories.FindAsync(id);
    }
}
