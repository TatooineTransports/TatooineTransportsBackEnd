using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Text.Json.Serialization;

namespace TTBackend.Models;

public class Vacation {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get; set;}
    public string userId {get; set;} = null!;
    public string title {get; set;} = null!;
    public int budget {get; set;}
    public string departDate {get; set;} = null!;
}