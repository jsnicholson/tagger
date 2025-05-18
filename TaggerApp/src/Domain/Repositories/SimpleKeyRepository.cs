using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface ISimpleKeyRepository<T> where T : class {
    public Task<T?> GetByIdAsync(Guid id);
    public Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids);
    public Task DeleteByIdAsync(Guid id);
    public Task DeleteByIdsAsync(IEnumerable<Guid> ids);
}

public class SimpleKeyRepository<T>(DbContext context) : GenericRepository<T>(context), ISimpleKeyRepository<T> where T : class {
    public async Task<T?> GetByIdAsync(Guid id) {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<Guid> ids) {
        var tasks = ids.Select(GetByIdAsync);
        var results = await Task.WhenAll(tasks);

        return results.Where(result => result != null)!;
    }

    public async Task DeleteByIdAsync(Guid id) {
        var entity = await GetByIdAsync(id);
        if (entity == null) {
            throw new KeyNotFoundException($"Entity with ID '{id}' was not found.");
        }

        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteByIdsAsync(IEnumerable<Guid> ids) {
        foreach (var id in ids) {
            await DeleteByIdAsync(id);
        }
    }
}
