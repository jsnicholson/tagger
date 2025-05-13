using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories {
    public interface IGenericRepository<T> where T : class {
        Task<T?> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task AddAsync(IEnumerable<T> entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(Guid id);
    }
    public class GenericRepository<T>(DbContext context) : IGenericRepository<T>
        where T : class
    {
        protected readonly DbContext Context = context;
        protected readonly DbSet<T> DbSet = context.Set<T>();

        public async Task<T?> GetByIdAsync(Guid id) {
            return await DbSet.FindAsync(id);
        }

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

        public async Task DeleteAsync(Guid id) {
            var entity = await DbSet.FindAsync(id);
            if(entity != null) {
                DbSet.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }
    }
}
