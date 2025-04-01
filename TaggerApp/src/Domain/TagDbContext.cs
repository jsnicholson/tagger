using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain {
    public class TagDbContext(DbContextOptions<TagDbContext> options) : DbContext(options)
    {
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Entities.File> Files { get; set; }
        public DbSet<TagOnFile> TagsOnFiles { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Tag>()
                .Property(t => t.id)
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
    }
}
