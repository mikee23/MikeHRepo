using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlbumWebApi.Models
{
    [Serializable, BsonIgnoreExtraElements]
    public class PhotoDetail
    {
        [BsonElement("photo_id"), BsonRepresentation(BsonType.Int32)]
        public int PhotoId { get; set; }
        [BsonElement("image_url"), BsonRepresentation(BsonType.String)]
        public string? ImageURL { get; set; } = String.Empty;
        [BsonElement("title"), BsonRepresentation(BsonType.String)]
        public string? Title { get; set; } = String.Empty;
    }
}
