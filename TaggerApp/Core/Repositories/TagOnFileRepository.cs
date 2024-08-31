using Core.Entities;
using Core.Repositories.Interfaces;

namespace Core.Repositories {
    public class TagOnFileRepository : GenericRepository<TagOnFile>, ITagOnFileRepository {
        public TagOnFileRepository(ApplicationDbContext context) : base(context) { }

        public async Task AddTagToFileAsync(Guid tagId, Guid fileId) {
            var tagOnFile = new TagOnFile { tagId = tagId, fileId = fileId };
            await _dbSet.AddAsync(tagOnFile);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTagFromFileAsync(Guid tagId, Guid fileId) {
            var tagOnFile = await _dbSet.FindAsync(tagId, fileId);
            if (tagOnFile != null) {
                _dbSet.Remove(tagOnFile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TagOnFile> GetTagOnFileAsync(Guid tagId, Guid fileId) {
            return await _dbSet.FindAsync(tagId, fileId);
        }
    }
}