using Microsoft.Extensions.DependencyInjection;

namespace Domain.Test
{
    public class BaseTest
    {
        protected TagDbContext DbContext { get; private set; } = null!;

        private TagDbContextFactory _contextFactory;

        [SetUp]
        public void SetUp()
        {
            // Initialize the factory with a database path for SQLite (you can provide a unique path for each test)
            _contextFactory = new TagDbContextFactory(new ServiceCollection().BuildServiceProvider());
            _contextFactory.SetDatabasePath($"DataSource=TestDb_{Guid.NewGuid()}.db");

            // Use the factory to create the DbContext
            DbContext = _contextFactory.CreateDbContext();

            // Ensure the database schema is created (for EF Core)
            DbContext.Database.EnsureCreated();
        }

        [TearDown]
        public void TearDown()
        {
            // Dispose of the DbContext and drop the SQLite database after the test
            DbContext.Dispose();
        }
    }
}