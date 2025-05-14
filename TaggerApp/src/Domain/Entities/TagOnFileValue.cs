using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("TagOnFileValue")]
public class TagOnFileValue : Entity {
    public TagOnFileValue() { } // test-compatible constructor

    public TagOnFileValue(Guid tagId, Guid fileId, string value) {
        TagId = tagId;
        FileId = fileId;
        Value = value;
    }

    [Required]
    public Guid TagId { get; set; }
    [Required]
    public Guid FileId { get; set; }
    [Required]
    [MaxLength(500)]
    public string Value { get; set; } // e.g. "5", "urgent", etc.

    [NotMapped]
    public TagOnFileId Id => new(TagId, FileId);

    // navigation properties
    public TagOnFile TagOnFile { get; set; } = null!;

    public override void ConfigureEntity(ModelBuilder modelBuilder) {
        var builder = modelBuilder.Entity<TagOnFileValue>();

        builder.HasKey(nameof(TagId), nameof(FileId));
    }
}
