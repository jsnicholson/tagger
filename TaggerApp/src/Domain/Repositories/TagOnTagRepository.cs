using Domain.Entities;

namespace Domain.Repositories;

public interface ITagOnTagRepository : ICompositeKeyRepository<TagOnTag, TagOnTagId> { }

public class TagOnTagRepository(TagDbContext context)
    : CompositeKeyRepository<TagOnTag, TagOnTagId>(context, id => [id.TaggerId, id.TaggerId]), ITagOnTagRepository { }
