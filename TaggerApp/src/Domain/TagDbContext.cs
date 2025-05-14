using System.Linq.Expressions;
using System.Reflection;

using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Domain {
    public class TagDbContext(DbContextOptions<TagDbContext> options) : DbContext(options) {
        public DbSet<Tag> Tags { get; init; }
        public DbSet<Entities.File> Files { get; init; }
        public DbSet<TagOnFile> TagsOnFiles { get; init; }
        public DbSet<TagOnFileValue> TagsOnFilesValues { get; init; }
        public DbSet<TagOnTag> TagsOnTags { get; init; }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            var baseEntityType = typeof(Entity);
            var entityTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t is { IsClass: true, IsAbstract: false } && baseEntityType.IsAssignableFrom(t)).ToList();

            ApplyGuidConversions(entityTypes, modelBuilder);
            ApplyOverriddenEntityConfiguration(entityTypes, modelBuilder);

            modelBuilder.Entity<TagOnTag>()
                .HasKey(tot => new { tot.TaggerId, tot.TaggedId });
        }

        private static void ApplyOverriddenEntityConfiguration(List<Type> entityTypes, ModelBuilder modelBuilder) {
            foreach (var entityType in entityTypes) {
                if (entityType.GetMethod("ConfigureEntity")?.DeclaringType == typeof(Entity)) {
                    continue;
                }

                var instance = Activator.CreateInstance(entityType) as Entity;
                instance?.ConfigureEntity(modelBuilder);
            }
        }

        private static void ApplyGuidConversions(List<Type> entityTypes, ModelBuilder modelBuilder) {
            var guidConverter = new ValueConverter<Guid, byte[]>(
                g => g.ToByteArray(),
                b => new Guid(b)
            );

            var nullableGuidConverter = new ValueConverter<Guid?, byte[]?>(
                g => g.HasValue ? g.Value.ToByteArray() : null,
                b => b != null ? new Guid(b) : (Guid?)null
            );

            foreach (var entityType in modelBuilder.Model.GetEntityTypes()) {
                foreach (var property in entityType.GetProperties()) {
                    if (property.ClrType == typeof(Guid)) {
                        property.SetValueConverter(guidConverter);
                    }
                    else if (property.ClrType == typeof(Guid?)) {
                        property.SetValueConverter(nullableGuidConverter);
                    }
                }
            }
        }

        public async Task SeedDataAsync() {
            // avoid re-seeding
            if (Files.Any()) {
                return;
            }

            await SeedFilesAsync();
            await SeedTagsAsync();
        }

        private async Task SeedFilesAsync() {
            var dbPath = new FileInfo(Database.GetDatabasePath());
            if (dbPath == null) {
                throw new ArgumentNullException($"Cannot seed to database at null path");
            }

            var absoluteFiles = FileSystemRepository.GetAllFilePaths(dbPath.DirectoryName!);
            var relativeFiles = absoluteFiles.Select(f => Path.GetRelativePath(dbPath.DirectoryName!, f)).ToList();
            var excludedFiles = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
                dbPath.Name,
                $"{dbPath.Name}-shm",
                $"{dbPath.Name}-wal"
            };
            var fileEntries = relativeFiles
                .Where(file => !excludedFiles.Contains(Path.GetFileName(file)))
                .Select(path => new Entities.File(path));

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
