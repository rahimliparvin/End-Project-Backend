using Savoy.Data;
using Savoy.Models;

namespace Savoy.Service.Interfaces
{
    public interface IContactService
    {

        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact> GetByIdAsync(int id);

    }
}
