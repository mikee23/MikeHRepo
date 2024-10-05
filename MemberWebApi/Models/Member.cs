using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace MemberWebApi.Models
{
    [Serializable, BsonIgnoreExtraElements]
    public class Member
    {
        [BsonId, BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
        public string MemberId { get; set; }
        [BsonElement("first_name"), BsonRepresentation(BsonType.String)]
        public string? FirstName { get; set; } = String.Empty;
        [BsonElement("last_name"), BsonRepresentation(BsonType.String)]
        public string? LastName { get; set; } = String.Empty;
        [BsonElement("email"), BsonRepresentation(BsonType.String)]
        public string? Email { get; set; } = String.Empty;
        [BsonElement("password"), BsonRepresentation(BsonType.String)]
        public string? Password { get; set; } = String.Empty;
        [BsonElement("date_added"), BsonRepresentation(BsonType.DateTime)]
        public DateTime? DateAdded { get; set; } = DateTime.Now;
        [BsonElement("photo_details")]
        public List<RoleDetails> RoleDetails { get; set; }
        /*public string? Password { get; set; } = String.Empty;
        [BsonElement("password_hash"), BsonRepresentation(BsonType.String)]
        public string? PasswordHash { get; set; } = String.Empty;
        [BsonElement("login_attempts"), BsonRepresentation(BsonType.Int32)]
        public int? LogAttempts { get; set; }*/
    }
}
