using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class LoginService : ILoginService
    {
        private readonly AppDbContext _context;

        public LoginService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Login> GetAsync() => await _context.Logins.Where(m=>!m.SoftDelete).SingleAsync();

        public async Task<Login> GetFullDataByIdAsync(int id) => await _context.Logins.FindAsync(id);
    
    }
}
