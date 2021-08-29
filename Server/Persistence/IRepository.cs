using System.Linq;
using System.Threading.Tasks;
using Server.Models;

namespace Server.Persistence {
    public interface IRepository<TModel> where TModel : Model {
        IQueryable AsQueryable();
        Task AddAsync(TModel model);
        Task UpdateAsync(TModel model);
        Task DeleteAsync(TModel model);
    }
}