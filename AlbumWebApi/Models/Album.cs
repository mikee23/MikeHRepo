using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumWebApi.Models
{
    [Serializable, BsonIgnoreExtraElements]
    public class Album
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string AlbumId { get; set; }
        [BsonElement("title"), BsonRepresentation(BsonType.String)]
        public string? Title { get; set; } = String.Empty;
        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public string? Description { get; set; } = String.Empty;
        [BsonElement("date_added"), BsonRepresentation(BsonType.DateTime)]
        public DateTime? DateAdded { get; set; } = DateTime.Now;
        [BsonElement("photo_details")]
        public List<PhotoDetail> PhotoDetails { get; set; }
    }
}
