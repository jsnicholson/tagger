namespace Meta.Domain.Repositories;

public interface IFileSystemRepository
{
    public IEnumerable<string> GetAllFiles(bool relative = false);
    public IEnumerable<string> GetAllSubfolders(bool relative = false);
}