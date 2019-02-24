using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pm.Data.Interfaces
{
    public interface IRepository<T> where T: class
    {
        Task<T> FindOne(object id);

        DbSet<T> Query();

        Task Create(T ent);

        Task CreateMany(IEnumerable<T> ents);

        Task Update(T ent);

        Task UpdateMany(IEnumerable<T> ents);

        Task Delete(T ent);

        Task DeleteMany(IEnumerable<T> ents);
    }
}
