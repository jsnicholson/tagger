using Domain.Repositories;
using Meta.Domain.Repositories;
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

            var optionsBuilder = new DbContextOptionsBuilder<TagDbContext>();
            optionsBuilder.UseSqlite($"Data Source={path.FullName}");

            using var context = new TagDbContext(optionsBuilder.Options);
            await context.Database.EnsureCreatedAsync();
        }

        public async Task InitialiseDatabaseAsync(FileInfo path, bool overwrite)
        {
            if (File.Exists(path.FullName) && !overwrite) {
                Console.WriteLine("Manifest already exists");
                return;
            }
            if(File.Exists(path.FullName) && overwrite) {
                File.Delete(path.FullName);
            }
            if(!Directory.Exists(path.DirectoryName)) Directory.CreateDirectory(path.DirectoryName);
            
            var optionsBuilder = new DbContextOptionsBuilder<TagDbContext>();
            optionsBuilder.UseSqlite($"Data Source={path.FullName}");

            using var context = new TagDbContext(optionsBuilder.Options);
            await context.Database.EnsureCreatedAsync();
            
            Debug.WriteLine($"Database created at: {path.FullName}");

            var absoluteFiles = FileSystemRepository.GetAllFilePaths(path.DirectoryName);
            var relativeFiles = absoluteFiles.Select(f => Path.GetRelativePath(path.DirectoryName, f)).ToList();
            var excludedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                path.Name,
                $"{path.Name}-shm",
                $"{path.Name}-wal"
            };
            var fileEntries = relativeFiles
                .Where(file => !excludedFiles.Contains(Path.GetFileName(file)))
                .Select(f => new Entities.File {
                id = Guid.NewGuid(),
                name = Path.GetFileName(f),
                extension = Path.GetExtension(f),
                path = f
            });

            var fileRepository = new FileRepository(context);
            await fileRepository.AddAsync(fileEntries);
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