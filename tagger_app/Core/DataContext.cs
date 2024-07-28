using Core.Entity;
using Microsoft.EntityFrameworkCore;

namespace Core {
    public class DataContext : DbContext {
        public DbSet<Tag> tags;
        public DbSet<Entity.File> files;
        public DbSet<TagOnFile> tagsOnFiles;
    }
}