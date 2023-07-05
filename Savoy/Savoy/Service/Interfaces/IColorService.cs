using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface IColorService
    {
        Task<IEnumerable<Color>> GetAllAsync();
        Task<Color> GetByIdAsync(int id);
    }
}
