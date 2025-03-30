using Microsoft.EntityFrameworkCore;

namespace Domain {
    public class DatabaseManager()
    {
        public static async Task InitialiseDatabaseAsync(string path)
        {
            if(!Directory.Exists(path)) Directory.CreateDirectory(path);
            var dbPath = Path.Combine(path, "tagger.db");
            var optionsBuilder = new DbContextOptionsBuilder<TagDbContext>();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            using var context = new TagDbContext(optionsBuilder.Options);
            await context.Database.MigrateAsync();
            
            Console.WriteLine($"Database created at: {dbPath}");
        }
    }
}