using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories {
    public class GenericRepository<T> : IGenericRepository<T> where T : class {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(DbContext context) {
            _context = context;
        }

        public async Task<T> GetByIdAsync(Guid id) {
            return await _dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllAsync() {
            return await _dbSet.ToListAsync();
        }

        public async Task AddAsync(T entity) {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity) {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id) {
            var entity = await _dbSet.FindAsync(id);
            if(entity != null) {
                _dbSet.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
