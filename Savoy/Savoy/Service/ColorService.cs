using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class ColorService : IColorService
    {
        private readonly AppDbContext _context;

        public ColorService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Color>> GetAllAsync() => await _context.Colors.Include(m=>m.ProductColors).  
                                                                                     ToListAsync();

        public async Task<Color> GetByIdAsync(int id) => await _context.Colors.FindAsync(id);

    }
}
