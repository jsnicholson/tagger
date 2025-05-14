using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories {
    public interface IGenericRepository<T> where T : class {
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task AddAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task UpdateAsync(IEnumerable<T> entities);
        Task DeleteAsync(T entity);
        Task DeleteAsync(IEnumerable<T> entities);
    }
    public class GenericRepository<T>(DbContext context) : IGenericRepository<T>
        where T : class {
        protected readonly DbContext Context = context;
        protected readonly DbSet<T> DbSet = context.Set<T>();

        public async Task<IEnumerable<T>> GetAllAsync() {
            return await DbSet.ToListAsync();
        }

        public async Task AddAsync(T entity) {
            await DbSet.AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task AddAsync(IEnumerable<T> entities) {
            await DbSet.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity) {
            DbSet.Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<T> entities) {
            DbSet.UpdateRange(entities);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity) {
            DbSet.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(IEnumerable<T> entities) {
            DbSet.RemoveRange(entities);
            await Context.SaveChangesAsync();
        }
    }
}
