using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain.Entities;

[Table("TagOnFile")]
public class TagOnFile(Guid tagId, Guid fileId) : Entity {
    [Required]
    [Column("TagId")]
    public Guid TagId { get; set; } = tagId;
    [Required]
    [Column("FileId")]
    public Guid FileId { get; set; } = fileId;

    // navigation properties
    [ForeignKey("TagId")] public Tag Tag { get; set; } = null!;
    [ForeignKey("FileId")] public File File { get; set; } = null!;
    public TagOnFileValue? Value { get; set; }

    public override void ConfigureEntity(ModelBuilder modelBuilder) {
        var builder = modelBuilder.Entity<TagOnFile>();
        
        builder.HasKey(nameof(FileId), nameof(TagId));

        builder.HasOne(t => t.File)
            .WithMany(f => f.TagsOnFile) // <-- match `File`'s collection
            .HasForeignKey(t => t.FileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Tag)
            .WithMany(tg => tg.TagOnFiles) // <-- match `Tag`'s collection
            .HasForeignKey(t => t.TagId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasOne(t => t.Value)
            .WithOne(v => v.TagOnFile)
            .HasForeignKey<TagOnFileValue>(v => new { v.FileId, v.TagId });

    }
}