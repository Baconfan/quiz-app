using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace QuizApi.Entities;

public class AppUser
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("username")]
    public required string UserName { get; set; }

    [BsonElement("displayedName")]
    public required string DisplayName { get; set; }
    
    [BsonElement("active")]
    public bool IsActive { get; set; }
    
    [BsonElement("roles")]
    public List<string> Roles { get; set; } = [];

    [BsonElement("passwordHash")] public byte[] PasswordHash { get; set; } = [];

    [BsonElement("passwordSalt")] public byte[] PasswordSalt { get; set; } = [];

}