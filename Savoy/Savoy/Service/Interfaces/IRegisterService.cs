using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface IRegisterService
    {
        Task<Register> GetAsync();
    }
}
