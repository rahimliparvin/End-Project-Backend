using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface ITeamService
    {
        Task<IEnumerable<Team>> GetAllAsync();
        Task<Team> GetFullDataByIdAsync(int id);
    }
}
