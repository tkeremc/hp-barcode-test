using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HPBarcodeTest.Models;

public class UserModel : BaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    public string? Email { get; set; }
    
    public string? PasswordHash { get; set; } //for JWT
}