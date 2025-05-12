using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("TagOnTag")]
public class TagOnTag(Guid taggerId, Guid taggedId) : Entity {
    [Required]
    [Column("TaggerId")]
    public Guid TaggerId { get; set; } = taggerId;

    [Required]
    [Column("TaggedId")]
    public Guid TaggedId { get; set; } = taggedId;

    public Tag Tagger { get; set; } = null!;
    public Tag Tagged { get; set; } = null!;

    public override void ConfigureEntity(ModelBuilder modelBuilder) {
        var builder = modelBuilder.Entity<TagOnTag>();

        builder.HasKey(t => new { t.TaggerId, t.TaggedId });

        builder.HasOne(t => t.Tagger)
            .WithMany(t => t.TagsTagged)
            .HasForeignKey(t => t.TaggerId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Tagged)
            .WithMany(t => t.TaggedBy)
            .HasForeignKey(t => t.TaggedId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}