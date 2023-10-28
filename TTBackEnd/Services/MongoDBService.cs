using TTBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;
using TTBackend.Controllers;
using Amazon.Runtime.Internal.Settings;
using System.Security.Cryptography.X509Certificates;

namespace TTBackend.Services;

public class MongoDBService {

    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<Vacation> _vacationsCollection;
    private readonly IMongoCollection<Event> _eventsCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings) {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _usersCollection = database.GetCollection<User>(mongoDBSettings.Value.UserCollectionName);
        _vacationsCollection = database.GetCollection<Vacation>(mongoDBSettings.Value.VacationCollectionName);
        _eventsCollection = database.GetCollection<Event>(mongoDBSettings.Value.EventCollectionName);
    }

    public async Task CreateUser(User user) {
        await _usersCollection.InsertOneAsync(user);
        return;
    }

    public async Task<User> GetUser(string id){
        List<User> users = await _usersCollection.Find(new BsonDocument()).ToListAsync();
        return users.Find(x => x.Id.Equals(id));
    }

    public async Task UpdateUser(string id, User user){
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("Id", id);
        await _usersCollection.FindOneAndReplaceAsync(filter, user);
        return;
    }

    public async Task DeleteUser(string id) {
        FilterDefinition<Event> eventFilter = Builders<Event>.Filter.Eq("userId", id);
        _eventsCollection.DeleteMany(eventFilter);
        FilterDefinition<Vacation> vacaFilter = Builders<Vacation>.Filter.Eq("userId", id);
        _vacationsCollection.DeleteMany(vacaFilter);
        FilterDefinition<User> userFilter = Builders<User>.Filter.Eq("Id", id);
        await _usersCollection.FindOneAndDeleteAsync(userFilter);
        return;
    }


    public async Task<List<Vacation>> GetVacations(string id) {
        List<Vacation> vacations = await _vacationsCollection.Find(new BsonDocument()).ToListAsync();
        return vacations.FindAll(x => x.userId.Equals(id));
        
    }

    public async Task CreateVacation(Vacation vacation) {
        await _vacationsCollection.InsertOneAsync(vacation);
        return;
    }

    public async Task UpdateVacation(string vacId, Vacation vacation) {
        FilterDefinition<Vacation> filter = Builders<Vacation>.Filter.Eq("Id", vacId);
        await _vacationsCollection.FindOneAndReplaceAsync(filter, vacation);
        return;
    }

    public async Task DeleteVacation(string vacId) {
        FilterDefinition<Vacation> filter = Builders<Vacation>.Filter.Eq("Id", vacId);
        await _vacationsCollection.FindOneAndDeleteAsync(filter);
        return;
    }
}