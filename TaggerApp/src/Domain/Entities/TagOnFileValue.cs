using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("TagOnFileValue")]
public class TagOnFileValue : Entity {
    public TagOnFileValue() {} // test-compatible constructor

    public TagOnFileValue(Guid fileId, Guid tagId, string value) {
        FileId = fileId;
        TagId = tagId;
        Value = value;
    }
    
    [Required] public Guid FileId { get; set; }

    [Required] public Guid TagId { get; set; }

    [Required] [MaxLength(500)] public string Value { get; set; } // e.g. "5", "urgent", etc.

    public TagOnFile TagOnFile { get; set; } = null!;

    public override void ConfigureEntity(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<TagOnFileValue>();

        builder.HasKey(x => new { x.FileId, x.TagId });
    }
}