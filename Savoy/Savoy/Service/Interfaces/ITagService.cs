using Savoy.Models;
using System.Collections;
using System.Drawing;

namespace Savoy.Service.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllAsync();

        Task<Tag> GetByIdAsync(int id);
    }
}
