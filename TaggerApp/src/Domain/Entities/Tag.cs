using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("Tag")]
public class Tag(string name) {
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
}