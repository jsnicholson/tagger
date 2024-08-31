using Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories {
    public class FileRepository : GenericRepository<Entities.File>, IFileRepository {
        public FileRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Entities.File>> GetFilesByTagAsync(Guid tagId) {
            return await _context.tagsOnFiles
                .Where(tof => tof.tagId == tagId)
                .Select(tof => tof.file)
                .ToListAsync();
        }
    }
}