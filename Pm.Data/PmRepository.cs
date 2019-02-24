using Pm.Data.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Pm.Data
{
    public class PmRepository<T>: IRepository<T> where T: class
    {
        private readonly PmEntities db;

        public PmRepository(PmEntities dbContext)
        {
            db = dbContext;
        }

        public async Task<T> FindOne(object id)
        {
            return await db.FindAsync<T>(id);
        }

        public DbSet<T> Query()
        {
            return db.Set<T>();
        }

        public async Task Create(T ent)
        {
            await db.AddAsync(ent);
            await db.SaveChangesAsync();
        }

        public async Task CreateMany(IEnumerable<T> ents)
        {
            db.AddRange(ents);
            await db.SaveChangesAsync();
        }

        public async Task Delete(T ent)
        {
            db.Remove(ent);
            await db.SaveChangesAsync();
        }

        public async Task DeleteMany(IEnumerable<T> ents)
        {
            db.RemoveRange(ents);
            await db.SaveChangesAsync();
        }

        public async Task Update(T ent)
        {
            db.Update(ent);
            await db.SaveChangesAsync();
        }

        public async Task UpdateMany(IEnumerable<T> ents)
        {
            db.UpdateRange(ents);
            await db.SaveChangesAsync();
        }
    }
}
