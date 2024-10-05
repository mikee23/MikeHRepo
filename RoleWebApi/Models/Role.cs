using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RoleWebApi.Models
{
    [Table("userrole")]
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("role_id")]
        public int RoleId { get; set; }
        [Column("description")]
        public string? Description { get; set; } = String.Empty;
        [Column("date_added")]
        public DateTime? DateAdded { get; set; } = DateTime.Now;
    }
}
