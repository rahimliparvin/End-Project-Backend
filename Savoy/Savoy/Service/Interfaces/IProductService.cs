using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> GetFullDataByIdAsync(int id);
    }
}
