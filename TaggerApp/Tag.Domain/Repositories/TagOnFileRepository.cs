using Domain.Entities;
using Domain.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories {
    public interface ITagOnFileRepository
    {
        public Task AddTagToFileAsync(Guid tagId, Guid fileId);
        public Task AddTagsToFileAsync(List<Guid> tagIds, Guid fileId);
        public Task AddTagsToFilesAsync(List<(Guid tagId, Guid fileId)> ids);
        public Task RemoveTagFromFileAsync(Guid tagId, Guid fileId);
        public Task RemoveTagsFromFileAsync(List<Guid> tagIds, Guid fileId);
    }
    
    public class TagOnFileRepository(TagDbContext context)
        : GenericRepository<TagOnFile>(context), ITagOnFileRepository
    {
        public async Task AddTagToFileAsync(Guid tagId, Guid fileId) {
            var tagOnFile = new TagOnFile { tagId = tagId, fileId = fileId };
            await _dbSet.AddAsync(tagOnFile);
            await _context.SaveChangesAsync();
        }

        public async Task AddTagsToFileAsync(List<Guid> tagIds, Guid fileId)
        {
            var tagsOnFile = tagIds.Select(t => new TagOnFile { tagId = t, fileId = fileId });
            await _dbSet.AddRangeAsync(tagsOnFile);
            await _context.SaveChangesAsync();
        }

        public async Task AddTagsToFilesAsync(List<(Guid tagId, Guid fileId)> ids)
        {
            var tagsOnFiles = ids.Select(t => new TagOnFile { tagId = t.tagId, fileId = t.fileId });
            await _dbSet.AddRangeAsync(tagsOnFiles);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveTagFromFileAsync(Guid tagId, Guid fileId) {
            var tagOnFile = await _dbSet.FindAsync(tagId, fileId);
            if (tagOnFile != null) {
                _dbSet.Remove(tagOnFile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveTagsFromFileAsync(List<Guid> tagIds, Guid fileId)
        {
            var tagsOnFile = await _dbSet.Where(tag => tagIds.Contains(tag.tagId) && tag.fileId == fileId).ToListAsync();
            if (tagsOnFile.Any())
            {
                _dbSet.RemoveRange(tagsOnFile);
                await _context.SaveChangesAsync();
            }
        }

        public async Task RemoveTagsFromFilesAsync(List<(Guid tagId, Guid fileId)> ids)
        {
            var tagsOnFiles = await _dbSet
                .Where(t => ids.Any(id => id.tagId == t.tagId && id.fileId == t.fileId))
                .ToListAsync();

            if (tagsOnFiles.Any())
            {
                _dbSet.RemoveRange(tagsOnFiles);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TagOnFile> GetTagOnFileAsync(Guid tagId, Guid fileId) {
            return await _dbSet.FindAsync(tagId, fileId);
        }
    }
}