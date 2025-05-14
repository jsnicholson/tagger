using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("TagOnTag")]
public class TagOnTag(Guid taggerId, Guid taggedId) : Entity {
    public TagOnTag() : this(Guid.Empty, Guid.Empty) { } // test-compatible constructor

    [Required]
    [Column("TaggerId")]
    public Guid TaggerId { get; set; } = taggerId;

    [Required]
    [Column("TaggedId")]
    public Guid TaggedId { get; set; } = taggedId;

    [NotMapped]
    public TagOnTagId Id => new(TaggerId, TaggedId);

    // navigation properties
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

public readonly struct TagOnTagId(Guid taggerId, Guid taggedId) : IEquatable<TagOnTagId> {
    public Guid TaggerId { get; } = taggerId;
    public Guid TaggedId { get; } = taggedId;

    public bool Equals(TagOnTagId other) =>
        TaggerId.Equals(other.TaggerId) && TaggedId.Equals(other.TaggedId);

    public override bool Equals(object? obj) =>
        obj is TagOnTagId other && Equals(other);

    public override int GetHashCode() =>
        HashCode.Combine(TaggerId, TaggedId);

    public static bool operator ==(TagOnTagId left, TagOnTagId right) => left.Equals(right);
    public static bool operator !=(TagOnTagId left, TagOnTagId right) => !left.Equals(right);

    public override string ToString() => $"({TaggerId}, {TaggedId})";
}
