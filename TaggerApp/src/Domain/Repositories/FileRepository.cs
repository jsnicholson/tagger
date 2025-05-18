using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Domain.Repositories {
    public interface IFileRepository : ISimpleKeyRepository<File> {
        Task<File?> GetFileByPath(string path, bool caseSensitive = false);
        Task<List<File>> GetFilesByPath(List<string> paths, bool caseSensitive = false);
        Task<File?> GetFileByPathContains(string substring, bool caseSensitive = false);
        Task<List<File>> GetFilesByPathContains(List<string> substrings, bool caseSensitive = false);
    }

    public class FileRepository(TagDbContext context)
        : SimpleKeyRepository<File>(context), IFileRepository {
        public async Task<File?> GetFileByPath(string path, bool caseSensitive = false) {
            Expression<Func<File, bool>> predicate = caseSensitive
                ? f => f.Path.Equals(path)
                : f => f.Path.Equals(path, StringComparison.CurrentCultureIgnoreCase);

            return await context.Set<File>().SingleOrDefaultAsync(predicate);
        }
        
        public async Task<List<File>> GetFilesByPath(List<string> paths, bool caseSensitive = false) {
            Expression<Func<File, bool>> predicate = caseSensitive
                ? f => paths.Contains(f.Path)
                : f => paths.Contains(f.Path, StringComparer.CurrentCultureIgnoreCase);
            
            return await context.Set<File>().Where(predicate).ToListAsync();
        }
        
        public async Task<File?> GetFileByPathContains(string substring, bool caseSensitive = false) {
            Expression<Func<File, bool>> predicate = caseSensitive
                ? f => f.Path.Contains(substring)
                : f => f.Path.Contains(substring, StringComparison.CurrentCultureIgnoreCase);
            
            return await context.Set<File>().SingleOrDefaultAsync(predicate);
        }


        public async Task<List<File>> GetFilesByPathContains(List<string> substrings, bool caseSensitive = false) {
            Expression<Func<File, bool>> predicate = caseSensitive
                ? f => substrings.Any(substring =>
                    f.Path.Contains(substring))
                : f => substrings.Any(substring =>
                    f.Path.Contains(substring, StringComparison.CurrentCultureIgnoreCase));
            
            return await context.Set<File>().Where(predicate).ToListAsync();
        }

    }
}
