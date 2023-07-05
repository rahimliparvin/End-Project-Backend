using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class SliderService : ISliderService
    {
        private readonly AppDbContext _context;
        public SliderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Slider>> GetAllAsync() => await _context.Sliders.Where(m => !m.SoftDelete).ToListAsync();

        public async Task<Slider> GetFullDataByIdAsync(int id) => await _context.Sliders.FindAsync(id);
    }
}
