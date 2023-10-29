using TTBackend.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using Microsoft.AspNetCore.Identity;
using TTBackend.Controllers;
using Amazon.Runtime.Internal.Settings;
using System.Security.Cryptography.X509Certificates;
using Amazon.Runtime.Internal;

namespace TTBackend.Services;

public class MongoDBService
{

    private readonly IMongoCollection<User> _usersCollection;
    private readonly IMongoCollection<Vacation> _vacationsCollection;
    private readonly IMongoCollection<Event> _eventsCollection;

    public MongoDBService(IOptions<MongoDBSettings> mongoDBSettings)
    {
        MongoClient client = new MongoClient(mongoDBSettings.Value.ConnectionURI);
        IMongoDatabase database = client.GetDatabase(mongoDBSettings.Value.DatabaseName);
        _usersCollection = database.GetCollection<User>(mongoDBSettings.Value.UserCollectionName);
        _vacationsCollection = database.GetCollection<Vacation>(mongoDBSettings.Value.VacationCollectionName);
        _eventsCollection = database.GetCollection<Event>(mongoDBSettings.Value.EventCollectionName);
    }

    /*
    * User Services
    */

    public async Task CreateUser(User user)
    {
        await _usersCollection.InsertOneAsync(user);
        return;
    }

    public async Task<User> GetUser(string uid)
    {
        List<User> users = await _usersCollection.Find(new BsonDocument()).ToListAsync();
        return users.Find(x => x.uid.Equals(uid));
    }

    public async Task<bool> ValidateUser(User user)
    {
        List<User> users = await _usersCollection.Find(new BsonDocument()).ToListAsync();
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("email", user.email);
        if (users.Any(x => x.email.Equals(user.email) || users.Any(x => x.username.Equals(user.username)) || users.Any(x => x.phone.Equals(user.phone))))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public async Task UpdateUser(string uid, User user)
    {
        FilterDefinition<User> filter = Builders<User>.Filter.Eq("uid", uid);
        await _usersCollection.FindOneAndReplaceAsync(filter, user);
        return;
    }

    public async Task DeleteUser(string uid)
    {
        FilterDefinition<Event> eventFilter = Builders<Event>.Filter.Eq("userUid", uid);
        _eventsCollection.DeleteMany(eventFilter);
        FilterDefinition<Vacation> vacaFilter = Builders<Vacation>.Filter.Eq("userUid", uid);
        _vacationsCollection.DeleteMany(vacaFilter);
        FilterDefinition<User> userFilter = Builders<User>.Filter.Eq("Id", uid);
        await _usersCollection.FindOneAndDeleteAsync(userFilter);
        return;
    }

    /*
    * Vacation Services
    */


    public async Task<List<Vacation>> GetVacations(string uid)
    {
        List<Vacation> vacations = await _vacationsCollection.Find(new BsonDocument()).ToListAsync();
        return vacations.FindAll(x => x.userUid.Equals(uid));

    }

    public async Task<Vacation> GetVacationById(string vacId)
    {
        List<Vacation> users = await _vacationsCollection.Find(new BsonDocument()).ToListAsync();
        return users.Find(x => x.Id.Equals(vacId));
    }

    public async Task CreateVacation(Vacation vacation)
    {
        await _vacationsCollection.InsertOneAsync(vacation);
        return;
    }

    public async Task UpdateVacation(string vacId, Vacation vacation)
    {
        FilterDefinition<Vacation> filter = Builders<Vacation>.Filter.Eq("Id", vacId);
        await _vacationsCollection.FindOneAndReplaceAsync(filter, vacation);
        return;
    }

    public async Task DeleteVacation(string vacId)
    {
        FilterDefinition<Vacation> filter = Builders<Vacation>.Filter.Eq("Id", vacId);
        await _vacationsCollection.FindOneAndDeleteAsync(filter);
        return;
    }

    /*
    *  Event Services
    */

    public async Task<List<Event>> GetEvents(string vacId)
    {
        List<Event> events = await _eventsCollection.Find(new BsonDocument()).ToListAsync();
        return events.FindAll(x => x.vacationId.Equals(vacId));
    }

    public async Task CreateEvent(Event newEvent)
    {
        await _eventsCollection.InsertOneAsync(newEvent);
        List<Vacation> vacations = await _vacationsCollection.Find(new BsonDocument()).ToListAsync();
        Vacation vacation = vacations.Find(x => x.Id.Equals(newEvent.vacationId));
        vacation.budget = vacation.budget - newEvent.cost;
        FilterDefinition<Vacation> filter = Builders<Vacation>.Filter.Eq("Id", vacation.Id);
        await _vacationsCollection.FindOneAndReplaceAsync(filter, vacation);
        return;
    }

    public async Task UpdateEvent(string eventId, Event updatedEvent)
    {
        List<Event> events = await _eventsCollection.Find(new BsonDocument()).ToListAsync();
        Event deletedEvent = events.Find(x => x.Id.Equals(eventId));
        List<Vacation> vacations = await _vacationsCollection.Find(new BsonDocument()).ToListAsync();
        Vacation vacation = vacations.Find(x => x.Id.Equals(deletedEvent.vacationId));
        vacation.budget = vacation.budget + deletedEvent.cost;

        FilterDefinition<Event> filter = Builders<Event>.Filter.Eq("Id", eventId);
        await _eventsCollection.FindOneAndReplaceAsync(filter, updatedEvent);

        events = await _eventsCollection.Find(new BsonDocument()).ToListAsync();
        deletedEvent = events.Find(x => x.Id.Equals(eventId));
        vacation.budget = vacation.budget - deletedEvent.cost;

        FilterDefinition<Vacation> filterVacation = Builders<Vacation>.Filter.Eq("Id", vacation.Id);
        await _vacationsCollection.FindOneAndReplaceAsync(filterVacation, vacation);
        return;
    }

    public async Task DeleteEvent(string eventId)
    {
        FilterDefinition<Event> filter = Builders<Event>.Filter.Eq("Id", eventId);

        List<Event> events = await _eventsCollection.Find(new BsonDocument()).ToListAsync();
        Event deletedEvent = events.Find(x => x.Id.Equals(eventId));

        List<Vacation> vacations = await _vacationsCollection.Find(new BsonDocument()).ToListAsync();
        Vacation vacation = vacations.Find(x => x.Id.Equals(deletedEvent.vacationId));
        vacation.budget = vacation.budget + deletedEvent.cost;
        FilterDefinition<Vacation> filterVacation = Builders<Vacation>.Filter.Eq("Id", vacation.Id);
        await _vacationsCollection.FindOneAndReplaceAsync(filterVacation, vacation);
        await _eventsCollection.FindOneAndDeleteAsync(filter);
        return;
    }
}