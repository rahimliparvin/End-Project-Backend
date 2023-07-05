using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class AboutService : IAboutService
    {
        private readonly AppDbContext _context;
        public AboutService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<About> GetAllAsync() => await _context.Abouts.Where(m=>!m.SoftDelete).SingleAsync();

    }
}
