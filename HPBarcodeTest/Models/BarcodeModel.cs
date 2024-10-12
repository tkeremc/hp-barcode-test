using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace HPBarcodeTest.Models;

public class BarcodeModel : BaseModel
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? MId { get; set; }
    public string? QrId { get; set; }
    public string? HpId { get; set; }
    public bool? IsDeleted { get; set; }
}