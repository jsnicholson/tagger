using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities {
    [Table("TagOnFile")]
    public class TagOnFile {
        [Column("TagId")]
        public Guid tagId { get; set; }
        [Column("FileId")]
        public Guid fileId { get; set; }

        // navigation properties
        [ForeignKey("tagId")]
        public Tag tag { get; set; }
        [ForeignKey("fileId")]
        public File file { get; set; }
    }
}
