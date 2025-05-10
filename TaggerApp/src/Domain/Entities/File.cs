using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("File")]
public class File(string path) {
    [Key]
    [Required]
    [Column("Id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [Column("Path")]
    [MaxLength(500)]
    public string Path { get; set; } = path;

    // navigation property
    public ICollection<TagOnFile> TagOnFiles { get; set; } = [];
}