using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("TagOnFile")]
public class TagOnFile(Guid tagId, Guid fileId) {
    [Required]
    [Column("TagId")]
    public Guid TagId { get; set; } = tagId;
    [Required]
    [Column("FileId")]
    public Guid FileId { get; set; } = fileId;

    // navigation properties
    [ForeignKey("TagId")]
    public Tag Tag { get; set; }
    [ForeignKey("FileId")]
    public File File { get; set; }
}