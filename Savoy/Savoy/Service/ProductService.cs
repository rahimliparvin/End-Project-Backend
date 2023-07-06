using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        public ProductService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsync() => await _context.Products.Include(m => m.ProductImages).
                                                                                         Include(m => m.ProductCategories).
                                                                                         ThenInclude(m => m.Category).
                                                                                         Include(m => m.ProductTags).
                                                                                         ThenInclude(m => m.Tag).
                                                                                         Include(m => m.ProductColors).
                                                                                         ThenInclude(m => m.Color).
                                                                                         ToListAsync();

       public async Task<Product> GetFullDataByIdAsync(int id) => await _context.Products.Include(m => m.ProductImages).
                                                                                   Include(m => m.ProductCategories).
                                                                                   ThenInclude(m=>m.Category).
                                                                                   Include(m => m.ProductTags).
                                                                                   ThenInclude(m=>m.Tag).
                                                                                   Include(m=>m.ProductColors).
                                                                                   ThenInclude(m => m.Color).
                                                                                   FirstOrDefaultAsync(m=>m.Id == id);
    }
}
