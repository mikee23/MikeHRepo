using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace PhotoWebApi.Models
{
    [Table("photo", Schema ="dbo")]
    public class Photo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("photo_id")]
        public int PhotoId { get; set; }
        [Column("image_url")]
        public string? ImageURL { get; set; } = String.Empty;
        [Column("title_id")]
        public string? Title { get; set; } = String.Empty;
        [Column("display_order")]
        public int DisplayOrder { get; set; }
        [Column("is_deleted")]
        public bool IsDeleted { get; set; }
        [Column("date_added")]
        public DateTime? DateAdded { get; set; } = DateTime.Now;
    }
}
