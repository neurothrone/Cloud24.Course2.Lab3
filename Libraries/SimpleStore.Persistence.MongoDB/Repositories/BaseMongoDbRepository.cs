using MongoDB.Bson;
using MongoDB.Driver;

namespace SimpleStore.Persistence.MongoDB.Repositories;

public class BaseMongoDbRepository
{
    private readonly string _connectionString;
    private readonly string _databaseName;

    protected BaseMongoDbRepository(string connectionString, string databaseName)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
    }

    protected IMongoCollection<BsonDocument> GetCollection(string collectionName)
    {
        var client = new MongoClient(_connectionString);
        var database = client.GetDatabase(_databaseName);
        var collection = database.GetCollection<BsonDocument>(collectionName);
        return collection;
    }
}