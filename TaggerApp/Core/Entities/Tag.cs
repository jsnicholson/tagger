using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities {
    [Table("Tag")]
    public class Tag {
        [Key]
        [Column("Id")]
        public Guid id { get; set; }
        [Required]
        [Column("Name")]
        [MaxLength(100)]
        public string name { get; set; }

        // navigation property
        public ICollection<TagOnFile> tagOnFiles { get; set; } = [];
    }
}