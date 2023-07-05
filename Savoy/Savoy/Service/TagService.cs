using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;
using System.Drawing;

namespace Savoy.Service
{
    public class TagService : ITagService
    {
        private readonly AppDbContext _context;

        public TagService(AppDbContext context)
        {
            _context= context;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync() => await _context.Tags.Include(m=>m.Blogs).Include(m=>m.ProductTags).ToListAsync();

        public async Task<Tag> GetByIdAsync(int id) => await _context.Tags.FindAsync(id);
      
    }
}
