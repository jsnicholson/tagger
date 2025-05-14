using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace Domain.Entities;

[Table("Tag")]
public class Tag : Entity {
    public Tag() { } // test-compatible constructor
    public Tag(string name) {
        Name = name;
    }

    [Key]
    [Required]
    [Column("Id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [Column("Name")]
    [MaxLength(100)]
    public string Name { get; set; }

    // navigation property
    public ICollection<TagOnFile> TagOnFiles { get; set; } = [];
    public ICollection<TagOnTag> TagsTagged { get; set; } = []; // Tags this tag applies
    public ICollection<TagOnTag> TaggedBy { get; set; } = [];    // Tags that apply to this tag
}
