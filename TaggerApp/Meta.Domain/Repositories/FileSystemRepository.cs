namespace Meta.Domain.Repositories;

public class FileSystemRepository : IFileSystemRepository
{
    private readonly string _rootPath;

    public FileSystemRepository(string rootPath)
    {
        if (!Directory.Exists(rootPath))
            throw new DirectoryNotFoundException($"The directory '{rootPath}' does not exist.");
        
        _rootPath = rootPath;
    }

    public IEnumerable<string> GetAllFiles(bool relative = false)
    {
        return Directory.GetFiles(_rootPath, "*.*", SearchOption.AllDirectories).Select(file => relative 
            ? Path.GetRelativePath(_rootPath, file).Replace(Path.DirectorySeparatorChar, '/')
            : file);
    }

    public IEnumerable<string> GetAllSubfolders(bool relative = false)
    {
        return Directory.GetDirectories(_rootPath, "*", SearchOption.AllDirectories).Select(folder => relative
            ? Path.GetRelativePath(_rootPath, folder).Replace(Path.DirectorySeparatorChar, '/')
            : folder);
    }
}