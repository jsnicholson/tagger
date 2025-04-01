using Microsoft.EntityFrameworkCore;

namespace Domain
{
    public class TagDbContextFactory {
        private readonly IServiceProvider _serviceProvider;
        private string? _databasePath;

        public TagDbContextFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }

        public void SetDatabasePath(string path) {
            _databasePath = path;
        }

        public TagDbContext CreateDbContext() {
            if (string.IsNullOrEmpty(_databasePath))
                throw new InvalidOperationException("Database path is not set!");

            var optionsBuilder = new DbContextOptionsBuilder<TagDbContext>();
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");

            return new TagDbContext(optionsBuilder.Options);
        }
    }
}