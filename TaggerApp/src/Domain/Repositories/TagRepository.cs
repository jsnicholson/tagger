using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories {
    public interface ITagRepository
    {
        public Task<IEnumerable<Tag>> GetTagsForFileAsync(Guid fileId);
    }
    
    public class TagRepository(TagDbContext context) : GenericRepository<Tag>(context), ITagRepository
    {
        public async Task<IEnumerable<Tag>> GetTagsForFileAsync(Guid fileId) {
            return await context.TagsOnFiles
                .Where(tof => tof.FileId == fileId)
                .Select(tof => tof.Tag)
                .ToListAsync();
        }
    }
}