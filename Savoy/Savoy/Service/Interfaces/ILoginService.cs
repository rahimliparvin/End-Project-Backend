using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface ILoginService
    {
        Task<Login> GetAsync();
        Task<Login> GetFullDataByIdAsync(int id);
    }
}