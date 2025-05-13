using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

[Table("File")]
public class File : Entity {
    public File() {} // test-compatible constructor

    public File(string path) {
        Path = path;
    }
    
    [Key]
    [Required]
    [Column("Id")]
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    [Column("Path")]
    [MaxLength(500)]
    public string Path { get; set; }

    // navigation property
    public ICollection<TagOnFile> TagsOnFile { get; set; } = [];
}