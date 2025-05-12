using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Tag")]
public class Tag(string name) : Entity {
    [Key]
    [Required]
    [Column("Id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [Column("Name")]
    [MaxLength(100)]
    public string Name { get; set; } = name;

    // navigation property
    public ICollection<TagOnFile> TagOnFiles { get; set; } = [];
    public ICollection<TagOnTag> TagsTagged { get; set; } = []; // Tags this tag applies
    public ICollection<TagOnTag> TaggedBy { get; set; } = [];    // Tags that apply to this tag
}