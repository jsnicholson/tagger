using System.Reflection;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using File = Domain.Entities.File;

namespace Domain.Test;

public class TestDbContext : DbContext {
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<File> Files => Set<File>();
    public DbSet<TagOnFile> TagsOnFiles => Set<TagOnFile>();
    public DbSet<TagOnFileValue> TagOnFileValues => Set<TagOnFileValue>();

    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        foreach (var entity in Assembly
                     .GetAssembly(typeof(Entity))!
                     .GetTypes()
                     .Where(t => t.IsClass && !t.IsAbstract && typeof(Entity).IsAssignableFrom(t))) {
            var instance = (Entity)Activator.CreateInstance(entity)!;
            instance.ConfigureEntity(modelBuilder);
        }
    }
}
