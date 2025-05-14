using Domain.Entities;

namespace Domain.Repositories;

public interface ITagOnFileRepository : ICompositeKeyRepository<TagOnFile, TagOnFileId> { }

public class TagOnFileRepository(TagDbContext context)
    : CompositeKeyRepository<TagOnFile, TagOnFileId>(context, id => [id.TagId, id.FileId]), ITagOnFileRepository { }
