using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity {
    /// <summary>
    /// an individual file on disk
    /// </summary>
    [Table("File")]
    public class File {
        [Key]
        public Guid id { get; set; }
        public string path { get; set; }
    }
}