using Domain.Entities;
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
            var tagOnFile = new TagOnFile(tagId, fileId);
            await DbSet.AddAsync(tagOnFile);
            await Context.SaveChangesAsync();
        }

        public async Task AddTagsToFileAsync(List<Guid> tagIds, Guid fileId)
        {
            var tagsOnFile = tagIds.Select(t => new TagOnFile(t, fileId));
            await DbSet.AddRangeAsync(tagsOnFile);
            await Context.SaveChangesAsync();
        }

        public async Task AddTagsToFilesAsync(List<(Guid tagId, Guid fileId)> ids)
        {
            var tagsOnFiles = ids.Select(t => new TagOnFile(t.tagId, t.fileId));
            await DbSet.AddRangeAsync(tagsOnFiles);
            await Context.SaveChangesAsync();
        }

        public async Task RemoveTagFromFileAsync(Guid tagId, Guid fileId) {
            var tagOnFile = await DbSet.FindAsync(tagId, fileId);
            if (tagOnFile != null) {
                DbSet.Remove(tagOnFile);
                await Context.SaveChangesAsync();
            }
        }

        public async Task RemoveTagsFromFileAsync(List<Guid> tagIds, Guid fileId)
        {
            var tagsOnFile = await DbSet.Where(tag => tagIds.Contains(tag.TagId) && tag.FileId == fileId).ToListAsync();
            if (tagsOnFile.Count != 0)
            {
                DbSet.RemoveRange(tagsOnFile);
                await Context.SaveChangesAsync();
            }
        }

        public async Task RemoveTagsFromFilesAsync(List<(Guid tagId, Guid fileId)> ids)
        {
            var tagsOnFiles = await DbSet
                .Where(t => ids.Any(id => id.tagId == t.TagId && id.fileId == t.FileId))
                .ToListAsync();

            if (tagsOnFiles.Count != 0)
            {
                DbSet.RemoveRange(tagsOnFiles);
                await Context.SaveChangesAsync();
            }
        }

        public async Task<TagOnFile?> GetTagOnFileAsync(Guid tagId, Guid fileId) {
            return await DbSet.FindAsync(tagId, fileId);
        }
    }
}