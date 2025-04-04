using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Domain {
    public class DatabaseManager(TagDbContextFactory _dbContextFactory) {
        public async Task CreateDatabaseAsync(FileInfo path, bool overwrite) {
            if (File.Exists(path.FullName) && !overwrite) {
                Console.WriteLine("Manifest already exists");
                return;
            }
            if (File.Exists(path.FullName) && overwrite) {
                DeleteDatabase(path.FullName);
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

            ConnectToDatabase(path.FullName);

            await _dbContextFactory.CreateDbContext().SeedDataAsync();
        }

        public void ConnectToDatabase(string path) {
            if(!File.Exists(path)) {
                throw new FileNotFoundException("Vault database not found", path);
            }

            _dbContextFactory.SetDatabasePath(path);
        }

        public void DeleteDatabase(string path) {
            _dbContextFactory.SetDatabasePath(null);
            File.Delete(path);
        }
    }
}