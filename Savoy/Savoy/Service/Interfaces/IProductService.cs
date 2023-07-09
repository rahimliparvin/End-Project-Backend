using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> GetFullDataByIdAsync(int id);

        Task<IEnumerable<Product>> GetPaginationDatas(int page, int take);

        Task<IEnumerable<Product>> GetPaginationData(int? categoryId,int? colorId,int? tagId ,int page, int take,string serachText);

        Task<int> GetCountAsync();
        Task<int> GetCategoryIdProductCountAsync(int? categoryId);
    }
}
