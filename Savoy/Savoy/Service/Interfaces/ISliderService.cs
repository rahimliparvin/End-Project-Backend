using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface ISliderService
    {
        Task<IEnumerable<Slider>> GetAllAsync();
        Task<Slider> GetFullDataByIdAsync(int id);
    }
}
