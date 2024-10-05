using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace JwtAuthenticationManager.Models
{
    [Serializable, BsonIgnoreExtraElements]
    public class RoleDetails
    {
        [BsonElement("role_id"), BsonRepresentation(BsonType.Int32)]
        public int RoleId { get; set; }
        [BsonElement("description"), BsonRepresentation(BsonType.String)]
        public string? Description { get; set; } = String.Empty;
    }
}
