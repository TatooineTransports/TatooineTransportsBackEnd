using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections;
using System.Text.Json.Serialization;

namespace TTBackend.Models;

public class Event {

    public string title {get; set;} = null!;
    public string vacationId {get; set;} = null!;
    public string userId {get; set;} = null!;
    public int cost {get; set;}
    public string timeOfEvent {get; set;} = null!;

}