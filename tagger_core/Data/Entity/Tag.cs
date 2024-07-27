using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity {
    /// <summary>
    /// individual tag to be applied to files
    /// </summary>
    [Table("Tag")]
    public class Tag {
        [Key]
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}