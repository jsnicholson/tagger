using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entity {
    [Table("File")]
    public class File {
        [Key]
        public Guid id { get; set; }
        public string path { get; set; }
    }
}