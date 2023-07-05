using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;
        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Contact>> GetAllAsync() => await _context.Contacts.Where(m => !m.SoftDelete).ToListAsync();

        public async Task<Contact> GetByIdAsync(int id) => await _context.Contacts.FindAsync(id);
    }
}
