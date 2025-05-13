using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Repositories;

public interface ITagOnTagRepository {
    Task AddTagOnTagAsync(Guid taggerId, Guid taggedId);
    Task RemoveTagFromTagAsync(Guid taggerId, Guid taggedId);
    Task<TagOnTag?> GetTagOnTagAsync(Guid taggerId, Guid taggedId);
    Task<IEnumerable<TagOnTag>> GetTagsForTaggerAsync(Guid taggerId);
    Task<IEnumerable<TagOnTag>> GetTagsForTaggedAsync(Guid taggedId);
}

public class TagOnTagRepository(TagDbContext context) : GenericRepository<TagOnTag>(context), ITagOnTagRepository
{
    public async Task AddTagOnTagAsync(Guid taggerId, Guid taggedId)
    {
        var tagOnTag = new TagOnTag(taggerId, taggedId);
        await DbSet.AddAsync(tagOnTag);
        await Context.SaveChangesAsync();
    }

    public async Task RemoveTagFromTagAsync(Guid taggerId, Guid taggedId)
    {
        var tagOnTag = await DbSet.FindAsync(taggerId, taggedId);
        if (tagOnTag != null)
        {
            DbSet.Remove(tagOnTag);
            await Context.SaveChangesAsync();
        }
    }

    public async Task<TagOnTag?> GetTagOnTagAsync(Guid taggerId, Guid taggedId)
    {
        return await DbSet.FindAsync(taggerId, taggedId);
    }

    public async Task<IEnumerable<TagOnTag>> GetTagsForTaggerAsync(Guid taggerId)
    {
        return await DbSet.Where(t => t.TaggerId == taggerId).ToListAsync();
    }

    public async Task<IEnumerable<TagOnTag>> GetTagsForTaggedAsync(Guid taggedId)
    {
        return await DbSet.Where(t => t.TaggedId == taggedId).ToListAsync();
    }
}