namespace Domain.Repositories {
    public interface IFileRepository : ISimpleKeyRepository<Entities.File> { }

    public class FileRepository(TagDbContext context)
        : SimpleKeyRepository<Entities.File>(context), IFileRepository { }
}
