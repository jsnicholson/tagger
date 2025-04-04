using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;
using Meta.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Domain {
    public class TagDbContext(DbContextOptions<TagDbContext> options) : DbContext(options)
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Entities.File> Files { get; set; }
        public DbSet<TagOnFile> TagsOnFiles { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Tag>()
                .Property(t => t.Id)
                .HasConversion(
                    v => v.ToByteArray(), // converts Guid to byte array (BLOB)
                    v => new Guid(v) // converts byte array (BLOB) to Guid
                );

            modelBuilder.Entity<Entities.File>()
                .Property(f => f.id)
                .HasConversion(
                    v => v.ToByteArray(), // converts Guid to byte array (BLOB)
                    v => new Guid(v) // converts byte array (BLOB) to Guid
                );

            modelBuilder.Entity<TagOnFile>()
                .HasKey(tof => new { tof.tagId, tof.fileId });

            modelBuilder.Entity<TagOnFile>()
                .Property(t => t.tagId)
                .HasConversion(
                    v => v.ToByteArray(), // converts Guid to byte array (BLOB)
                    v => new Guid(v) // converts byte array (BLOB) to Guid
                );

            modelBuilder.Entity<TagOnFile>()
                .Property(f => f.fileId)
                .HasConversion(
                    v => v.ToByteArray(), // converts Guid to byte array (BLOB)
                    v => new Guid(v) // converts byte array (BLOB) to Guid
                );
        }

        public async Task SeedDataAsync() {
            // avoid re-seeding
            if (Files.Any()) return;
            

            await SeedFilesAsync();
            await SeedTagsAsync();
        }

        private async Task SeedFilesAsync() {
            var dbPath = new FileInfo(Database.GetDatabasePath());
            var absoluteFiles = FileSystemRepository.GetAllFilePaths(dbPath.DirectoryName);
            var relativeFiles = absoluteFiles.Select(f => Path.GetRelativePath(dbPath.DirectoryName, f)).ToList();
            var excludedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                dbPath.Name,
                $"{dbPath.Name}-shm",
                $"{dbPath.Name}-wal"
            };
            var fileEntries = relativeFiles
                .Where(file => !excludedFiles.Contains(Path.GetFileName(file)))
                .Select(f => new Entities.File {
                    id = Guid.NewGuid(),
                    name = Path.GetFileName(f),
                    extension = Path.GetExtension(f).Substring(1),
                    path = f
                });

            var fileRepository = new FileRepository(this);
            await fileRepository.AddAsync(fileEntries);
        }

        private async Task SeedTagsAsync() {
            string[] tagNames = [
                "favourite",
                "liked",
                "media",
                "tv",
                "film",
                "book",
                "image",
                "funny"
            ];
            var tags = tagNames.Select(t => new Tag(t));

            var tagRepository = new TagRepository(this);
            await tagRepository.AddAsync(tags);
        }
    }
}
