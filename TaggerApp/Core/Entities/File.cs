using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities {
    [Table("File")]
    public class File {
        [Key]
        [Column("Id")]
        public Guid id { get; set; }
        [Required]
        [Column("Name")]
        [MaxLength(255)]
        public string name { get; set; }
        [Required]
        [Column("Extension")]
        [MaxLength(10)]
        public string extension { get; set; }
        [Required]
        [Column("Path")]
        [MaxLength(500)]
        public string path { get; set; }

        // navigation property
        public ICollection<TagOnFile> tagOnFiles { get; set; } = new List<TagOnFile>();
    }
}
