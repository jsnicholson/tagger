using System.Diagnostics;

namespace Domain;

public interface IDatabaseManager {
    Task CreateDatabaseAsync(FileInfo path, bool overwrite);
    Task InitialiseDatabaseAsync(FileInfo path, bool overwrite);
    TagDbContext ConnectToDatabase(FileInfo path);
    void DeleteDatabase(FileInfo path);
}

public class DatabaseManager(TagDbContextFactory dbContextFactory) : IDatabaseManager {
    public async Task CreateDatabaseAsync(FileInfo path, bool overwrite) {
        if (File.Exists(path.FullName) && !overwrite) {
            Debug.WriteLine("Manifest already exists");
            return;
        }
        if (File.Exists(path.FullName) && overwrite)
            DeleteDatabase(path);
        
        if (!Directory.Exists(path.DirectoryName) && path.DirectoryName != null)
            Directory.CreateDirectory(path.DirectoryName);

        dbContextFactory.SetDatabasePath(path.FullName);

        await using var context = dbContextFactory.CreateDbContext();
        await context.Database.EnsureCreatedAsync();

        Debug.WriteLine($"Database created at: {path.FullName}");
    }

    public async Task InitialiseDatabaseAsync(FileInfo path, bool overwrite)
    {
        await CreateDatabaseAsync(path, overwrite);

        await using var context = ConnectToDatabase(path);

        await context.SeedDataAsync();
    }

    public TagDbContext ConnectToDatabase(FileInfo path) {
        if(!File.Exists(path.FullName)) {
            throw new FileNotFoundException("Database not found", path.FullName);
        }

        dbContextFactory.SetDatabasePath(path.FullName);

        return dbContextFactory.CreateDbContext();
    }

    public void DeleteDatabase(FileInfo path) {
        dbContextFactory.SetDatabasePath(null);
        File.Delete(path.FullName);
    }
}