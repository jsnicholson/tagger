using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public ICollection<TagOnFile> TagOnFiles { get; set; } = []; // Relation between Tags and Files
    public ICollection<File> TaggedFiles => TagOnFiles.Select(tof => tof.File).ToList(); // Files tagged by this tag
    [NotMapped]
    public ICollection<Tag> TagsTagged => TagOnTags.Select(t => t.Source).ToList();
    [NotMapped]
    public ICollection<Tag> TaggedBy => TagsOnTag.Select(t => t.Target).ToList();
    public ICollection<TagOnTag> TagOnTags { get; set; } = []; // Tags this tag applies
    public ICollection<TagOnTag> TagsOnTag { get; set; } = [];    // Tags that apply to this tag
}
