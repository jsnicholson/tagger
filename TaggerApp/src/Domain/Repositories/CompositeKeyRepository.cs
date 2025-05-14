using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface ICompositeKeyRepository<T, TKey> where T : class {
    Task<T?> GetByIdAsync(TKey id);
    Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<TKey> ids);
    Task DeleteByIdAsync(TKey id);
    Task DeleteByIdsAsync(IEnumerable<TKey> ids);
}

public class CompositeKeyRepository<T, TKey>(DbContext context, Func<TKey, object[]> keySelector)
    : GenericRepository<T>(context), ICompositeKeyRepository<T, TKey>
    where T : class {
    public async Task<T?> GetByIdAsync(TKey id) {
        var keys = keySelector(id);
        return await DbSet.FindAsync(keys);
    }

    public async Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<TKey> ids) {
        var tasks = ids.Select(GetByIdAsync);
        var results = await Task.WhenAll(tasks);
        return results.Where(r => r != null)!;
    }

    public async Task DeleteByIdAsync(TKey id) {
        var entity = await GetByIdAsync(id);
        if (entity == null) {
            throw new KeyNotFoundException($"Entity with ID '{id}' was not found.");
        }

        DbSet.Remove(entity);
        await Context.SaveChangesAsync();
    }

    public async Task DeleteByIdsAsync(IEnumerable<TKey> ids) {
        var tasks = ids.Select(GetByIdAsync);
        var results = await Task.WhenAll(tasks);
        var nonNull = results.Where(e => e != null);
        DbSet.RemoveRange(nonNull!);
        await Context.SaveChangesAsync();
    }
}
