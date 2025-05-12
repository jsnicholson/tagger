using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities;

[Table("TagOnFileValue")]
public class TagOnFileValue(Guid fileId, Guid tagId, string value) : Entity
{
    [Required] public Guid FileId { get; set; } = fileId;

    [Required] public Guid TagId { get; set; } = tagId;

    [Required] [MaxLength(500)] public string Value { get; set; } = value; // e.g. "5", "urgent", etc.

    public TagOnFile TagOnFile { get; set; } = null!;

    public override void ConfigureEntity(ModelBuilder modelBuilder)
    {
        var builder = modelBuilder.Entity<TagOnFileValue>();

        builder.HasKey(x => new { x.FileId, x.TagId });

        builder.HasOne(x => x.TagOnFile)
            .WithOne() // optionally: .WithOne(tof => tof.Value)
            .HasForeignKey<TagOnFileValue>(x => new { x.FileId, x.TagId })
            .OnDelete(DeleteBehavior.Cascade);
    }
}