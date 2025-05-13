using Domain.Library;

namespace Domain.Repositories;

public static class FileSystemRepository {
    public static IEnumerable<string> GetAllFilePaths(string path) {
        return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories).ToList();
    }

    public static IEnumerable<string> GetAllSubfolders(string path) {
        return Directory.GetDirectories(path, "*", SearchOption.AllDirectories).ToList();
    }

    public static void GetFileMetadata(string path) {
        var fileInfo = new FileInfo(path);
        var obj = new {
            extension = fileInfo.Extension,
            type = FileTypeMap.GetFileTypeInfo(path),
            size = fileInfo.Length,
            lastModified = fileInfo.LastWriteTimeUtc,
            createdData = fileInfo.CreationTimeUtc
        };
    }
}