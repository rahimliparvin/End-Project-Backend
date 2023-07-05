using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface ILoginService
    {
        Task<Login> GetAsync();
    }
}