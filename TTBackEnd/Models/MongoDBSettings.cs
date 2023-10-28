namespace TTBackend.Models;

public class MongoDBSettings {

    public string ConnectionURI {get; set;} = null!;
    public string DatabaseName {get; set;} = null!;
    public string UserCollectionName {get; set;} = null!;
    public string VacationCollectionName {get; set;} = null!;
    public string EventCollectionName {get; set;} = null!;

}