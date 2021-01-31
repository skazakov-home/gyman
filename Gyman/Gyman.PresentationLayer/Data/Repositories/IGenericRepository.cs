using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gyman.PresentationLayer.Data.Repositories
{
    public interface IGenericRepository<T>
    {
        void Add(T model);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        bool HasChanges();
        void Remove(T model);
        Task SaveAsync();
    }
}