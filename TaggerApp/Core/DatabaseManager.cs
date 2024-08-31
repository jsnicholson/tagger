using Microsoft.EntityFrameworkCore;

namespace Core {
    public class DatabaseManager {
        public static ApplicationDbContext CreateDbContext(string dbFilePath) {
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlite($"Data Source={dbFilePath}");

            var context = new ApplicationDbContext(optionsBuilder.Options);

            context.Database.EnsureCreated();

            return context;
        }
    }
}