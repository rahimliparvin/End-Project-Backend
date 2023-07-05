using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface IAboutService
    {
        Task<About> GetAllAsync();
    }
}
