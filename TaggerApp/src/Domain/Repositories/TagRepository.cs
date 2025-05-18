using System.Linq.Expressions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Domain.Repositories {
    public interface ITagRepository {
        Task<Tag?> GetTagByName(string name, bool caseInsensitive = false);
        Task<List<Tag>> GetTagsByName(List<string> names, bool caseInsensitive = false);
        Task<Tag?> GetTagByNameContains(string substring, bool caseInsensitive = false);
        Task<List<Tag>> GetTagsByNamesContains(List<string> substrings, bool caseInsensitive = false);
    }

    public class TagRepository(TagDbContext context) : GenericRepository<Tag>(context), ITagRepository {
        public async Task<Tag?> GetTagByName(string name, bool caseInsensitive = false) {
            Expression<Func<Tag, bool>> predicate = caseInsensitive
                ? t => t.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase)
                : t => t.Name.Equals(name);

            return await context.Set<Tag>().SingleOrDefaultAsync(predicate);
        }

        public async Task<List<Tag>> GetTagsByName(List<string> names, bool caseInsensitive = false) {
            Expression<Func<Tag, bool>> predicate = caseInsensitive
                ? t => names.Contains(t.Name, StringComparer.CurrentCultureIgnoreCase)
                : t => names.Contains(t.Name);
            
            return await context.Set<Tag>().Where(predicate).ToListAsync();
        }

        public async Task<Tag?> GetTagByNameContains(string substring, bool caseInsensitive = false) {
            Expression<Func<Tag, bool>> predicate = caseInsensitive
                ? t => t.Name.Contains(substring, StringComparison.CurrentCultureIgnoreCase)
                : t => t.Name.Contains(substring);
            
            return await context.Set<Tag>().SingleOrDefaultAsync(predicate);
        }

        public async Task<List<Tag>> GetTagsByNamesContains(List<string> substrings, bool caseInsensitive = false) {
            Expression<Func<Tag, bool>> predicate = caseInsensitive
                ? t => substrings.Any(substring =>
                    t.Name.Contains(substring, StringComparison.CurrentCultureIgnoreCase))
                : t => substrings.Any(substring =>
                    t.Name.Contains(substring));
            
            return await context.Set<Tag>().Where(predicate).ToListAsync();
        }
    }
}
