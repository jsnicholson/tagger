using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories {
    public interface IFileRepository
    {
        public Task<IEnumerable<Entities.File>> GetFilesByTagAsync(Guid tagId);
    }
    
    public class FileRepository(TagDbContext context)
        : GenericRepository<Entities.File>(context), IFileRepository
    {
        public async Task<IEnumerable<Entities.File>> GetFilesByTagAsync(Guid tagId) {
            return await context.TagsOnFiles
                .Where(tof => tof.tagId == tagId)
                .Select(tof => tof.file)
                .ToListAsync();
        }
    }
}