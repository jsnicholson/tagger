using System.Diagnostics;

namespace Domain;

public interface IDatabaseManager {
    Task CreateDatabaseAsync(FileInfo path, bool overwrite);
    Task InitialiseDatabaseAsync(FileInfo path, bool overwrite);
    TagDbContext ConnectToDatabase(FileInfo path);
    void DeleteDatabase(FileInfo path);
}

public class DatabaseManager(TagDbContextFactory _dbContextFactory) : IDatabaseManager {
    public async Task CreateDatabaseAsync(FileInfo path, bool overwrite) {
        if (File.Exists(path.FullName) && !overwrite) {
            Console.WriteLine("Manifest already exists");
            return;
        }
        if (File.Exists(path.FullName) && overwrite) {
            DeleteDatabase(path);
        }
        if (!Directory.Exists(path.DirectoryName)) Directory.CreateDirectory(path.DirectoryName);

        _dbContextFactory.SetDatabasePath(path.FullName);

        using var context = _dbContextFactory.CreateDbContext();
        await context.Database.EnsureCreatedAsync();

        Debug.WriteLine($"Database created at: {path.FullName}");
    }

    public async Task InitialiseDatabaseAsync(FileInfo path, bool overwrite)
    {
        await CreateDatabaseAsync(path, overwrite);

        using var context = ConnectToDatabase(path);

        await context.SeedDataAsync();
    }

    public TagDbContext ConnectToDatabase(FileInfo path) {
        if(!File.Exists(path.FullName)) {
            throw new FileNotFoundException("Database not found", path.FullName);
        }

        _dbContextFactory.SetDatabasePath(path.FullName);

        return _dbContextFactory.CreateDbContext();
    }

    public void DeleteDatabase(FileInfo path) {
        _dbContextFactory.SetDatabasePath(null);
        File.Delete(path.FullName);
    }
}