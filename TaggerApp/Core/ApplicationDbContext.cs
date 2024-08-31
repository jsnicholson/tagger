using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core {
    public class ApplicationDbContext : DbContext {
        public DbSet<Tag> tags { get; set; }
        public DbSet<Entities.File> files { get; set; }
        public DbSet<TagOnFile> tagsOnFiles { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

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
