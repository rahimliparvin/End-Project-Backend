using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class RegisterService : IRegisterService
    {
        private readonly AppDbContext _context;

        public RegisterService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Register> GetAsync() => await _context.Registers.Where(m => !m.SoftDelete).SingleAsync();

        public async Task<Register> GetFullDataByIdAsync(int id) => await _context.Registers.FindAsync(id);

    }
}
