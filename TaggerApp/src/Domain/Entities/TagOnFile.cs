using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("TagOnFile")]
public class TagOnFile : Entity {
    public TagOnFile() { } // EF-compatible constructor
    public TagOnFile(Guid tagId, Guid fileId, string? value = null) {
        TagId = tagId;
        FileId = fileId;
        Value = value;
    }

    [Required]
    [Column("TagId")]
    public Guid TagId { get; set; }
    [Required]
    [Column("FileId")]
    public Guid FileId { get; set; }
    [Column("Value")]
    [MaxLength(500)]
    public string? Value { get; set; }
    [NotMapped]
    public TagOnFileId Id => new(TagId, FileId);


    // navigation properties
    public Tag Tag { get; set; } = null!;
    public File File { get; set; } = null!;

    public override void ConfigureEntity(ModelBuilder modelBuilder) {
        var builder = modelBuilder.Entity<TagOnFile>();

        builder.HasKey(nameof(TagId), nameof(FileId));

        builder.HasOne(t => t.File)
            .WithMany(f => f.TagsOnFile) // <-- match `File`'s collection
            .HasForeignKey(t => t.FileId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(t => t.Tag)
            .WithMany(t => t.TagOnFiles) // <-- match `Tag`'s collection
            .HasForeignKey(t => t.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public readonly struct TagOnFileId(Guid tagId, Guid fileId) : IEquatable<TagOnFileId> {
    public Guid TagId { get; } = tagId;
    public Guid FileId { get; } = fileId;

    // Value equality
    public bool Equals(TagOnFileId other) =>
        TagId.Equals(other.TagId) && FileId.Equals(other.FileId);

    public override bool Equals(object? obj) =>
        obj is TagOnFileId other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(TagId, FileId);

    public static bool operator ==(TagOnFileId left, TagOnFileId right) => left.Equals(right);
    public static bool operator !=(TagOnFileId left, TagOnFileId right) => !left.Equals(right);

    public override string ToString() => $"({TagId}, {FileId})";
}
