using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities {
    [Table("File")]
    public class File {
        [Key]
        [Column("Id")]
        public Guid Id { get; set; }
        [Required]
        [Column("Path")]
        [MaxLength(500)]
        public string Path { get; set; }

        // navigation property
        public ICollection<TagOnFile> tagOnFiles { get; set; } = new List<TagOnFile>();
    }
}
