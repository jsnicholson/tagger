using Domain.Entities;
using Domain.Extensions;
using Domain.Repositories;
using Meta.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Domain {
    public class TagDbContext(DbContextOptions<TagDbContext> options) : DbContext(options)
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Entities.File> Files { get; set; }
        public DbSet<TagOnFile> TagsOnFiles { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var baseEntityType = typeof(Entity);
            var entityTypes = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && baseEntityType.IsAssignableFrom(t));
            
            ApplyGuidConversions(entityTypes, modelBuilder);
            ApplyOverriddenEntityConfiguration(entityTypes, modelBuilder);
            
            modelBuilder.Entity<TagOnTag>()
                .HasKey(tot => new { tot.TaggerId, tot.TaggedId });
        }

        private static void ApplyOverriddenEntityConfiguration(IEnumerable<Type> entityTypes, ModelBuilder modelBuilder)  {
            foreach (var entityType in entityTypes) {
                if (entityType.GetMethod("ConfigureEntity")?.DeclaringType == typeof(Entity)) continue;

                var instance = Activator.CreateInstance(entityType) as Entity;
                instance?.ConfigureEntity(modelBuilder);
            }
        }

        private static void ApplyGuidConversions(IEnumerable<Type> entityTypes, ModelBuilder modelBuilder) {
            foreach (var entityType in entityTypes) {
                var entityBuilder = modelBuilder.Entity(entityType);
                ApplyGuidConversion(entityType, entityBuilder);
            }
        }
        
        private static void ApplyGuidConversion(Type entityType, EntityTypeBuilder builder) {
            var guidProps = entityType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(Guid));

            foreach (var prop in guidProps)
            {
                // Use generic method to create a PropertyBuilder<Guid>
                var method = typeof(EntityTypeBuilder)
                    .GetMethods()
                    .First(m => m.Name == "Property" && m.GetParameters().Length == 1)
                    .MakeGenericMethod(typeof(Guid));

                var propertyBuilder = method.Invoke(builder, new object[] { prop.Name });

                // Get the HasConversion method from PropertyBuilder<Guid>
                var hasConversionMethod = propertyBuilder
                    .GetType()
                    .GetMethods()
                    .First(m => m.Name == "HasConversion" &&
                                m.GetParameters().Length == 2 &&
                                m.GetParameters()[0].ParameterType.Name.StartsWith("Expression"));

                // Build lambda expressions: Guid => byte[], byte[] => Guid
                var toProvider = (Func<Guid, byte[]>) (g => g.ToByteArray());
                var fromProvider = (Func<byte[], Guid>) (b => new Guid(b));

                // Call HasConversion with lambdas
                hasConversionMethod.Invoke(propertyBuilder, [toProvider, fromProvider]);
            }
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
