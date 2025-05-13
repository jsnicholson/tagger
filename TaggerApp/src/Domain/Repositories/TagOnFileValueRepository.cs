using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface ITagOnFileValueRepository
{
    Task AddValueToTagOnFileAsync(Guid fileId, Guid tagId, string value);
    Task RemoveTagValueFromFileAsync(Guid fileId, Guid tagId);
    Task<TagOnFileValue?> GetTagValueForFileAsync(Guid fileId, Guid tagId);
    Task<IEnumerable<TagOnFileValue>> GetValuesForFileAsync(Guid fileId);
    Task<IEnumerable<TagOnFileValue>> GetValuesForTagAsync(Guid tagId);
}

public class TagOnFileValueRepository(TagDbContext context) : GenericRepository<TagOnFileValue>(context), ITagOnFileValueRepository {
    public async Task AddValueToTagOnFileAsync(Guid fileId, Guid tagId, string value)
    {
        var tagOnFileValue = new TagOnFileValue(fileId, tagId, value);
        await DbSet.AddAsync(tagOnFileValue);
        await Context.SaveChangesAsync();
    }

    public async Task RemoveTagValueFromFileAsync(Guid fileId, Guid tagId)
    {
        var tagOnFileValue = await DbSet.FindAsync(fileId, tagId);
        if (tagOnFileValue != null)
        {
            DbSet.Remove(tagOnFileValue);
            await Context.SaveChangesAsync();
        }
    }

    public async Task<TagOnFileValue?> GetTagValueForFileAsync(Guid fileId, Guid tagId)
    {
        return await DbSet.FindAsync(fileId, tagId);
    }

    public async Task<IEnumerable<TagOnFileValue>> GetValuesForFileAsync(Guid fileId)
    {
        return await DbSet.Where(t => t.FileId == fileId).ToListAsync();
    }

    public async Task<IEnumerable<TagOnFileValue>> GetValuesForTagAsync(Guid tagId)
    {
        return await DbSet.Where(t => t.TagId == tagId).ToListAsync();
    }
}