using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity {
    [Table("Tag")]
    public class Tag {
        [Key]
        public Guid id { get; set; }
        public string name { get; set; }
    }
}