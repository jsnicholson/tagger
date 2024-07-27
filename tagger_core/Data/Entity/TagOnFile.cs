using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entity {
    [Table("TagOnFile")]
    public class TagOnFile {
        [Key, Column(Order = 0)]
        public Guid tagId { get; set; }
        [ForeignKey("tagId")]
        public Tag tag { get; set; }
        [Key, Column(Order = 1)]
        public Guid fileId { get; set; }
        [ForeignKey("fileId")]
        public File file { get; set; }
    }
}