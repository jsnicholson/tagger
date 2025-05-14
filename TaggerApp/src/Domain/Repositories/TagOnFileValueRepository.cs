using Domain.Entities;

namespace Domain.Repositories;

public interface ITagOnFileValueRepository : ICompositeKeyRepository<TagOnFileValue, TagOnFileId> { }

public class TagOnFileValueRepository(TagDbContext context)
    : CompositeKeyRepository<TagOnFileValue, TagOnFileId>(context, id => [id.TagId, id.FileId]), ITagOnFileValueRepository { }
