using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAsync();
    }
}
