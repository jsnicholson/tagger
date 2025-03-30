using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Meta.Domain.Entities  {
    [Table("Vault")]
    public class Vault {
        [Key]
        [Column("")]
        public Guid id { get; set; }
    }
}