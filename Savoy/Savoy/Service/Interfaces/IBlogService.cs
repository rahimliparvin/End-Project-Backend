using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllAsync();
        Task<Blog> GetFullDataByIdAsync(int id);
    }
}
