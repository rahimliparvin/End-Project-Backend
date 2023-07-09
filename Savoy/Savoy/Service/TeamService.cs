using Microsoft.EntityFrameworkCore;
using Savoy.Data;
using Savoy.Models;
using Savoy.Service.Interfaces;

namespace Savoy.Service
{
    public class TeamService : ITeamService
    {
        private readonly AppDbContext _context;
        public TeamService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Team>> GetAllAsync() => await _context.Teams.ToListAsync();

        public async Task<Team> GetFullDataByIdAsync(int id) => await _context.Teams.FindAsync(id);
    }
}
