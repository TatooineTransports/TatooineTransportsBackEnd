using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Text.Json.Serialization;

namespace TTBackend.Models;

public class User {

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id {get; set;}
    public string uid {get; set;} = null!;
    public string username {get; set;} = null!;
    public string email {get; set;} = null!;
    public string phone {get; set;} = null!;
}