using Core.Entities;
using Core.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Core.Repositories {
    public class TagRepository : GenericRepository<Tag>, ITagRepository {
        public TagRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Tag>> GetTagsForFileAsync(Guid fileId) {
            return await _context.tagsOnFiles
                .Where(tof => tof.fileId == fileId)
                .Select(tof => tof.tag)
                .ToListAsync();
        }
    }
}