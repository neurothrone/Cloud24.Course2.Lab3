using MongoDB.Bson;
using MongoDB.Driver;

namespace SimpleStore.Persistence.MongoDB.Repositories;

public class BaseMongoDbRepository
{
    private readonly string _connectionString;
    private readonly string _databaseName;
    private readonly bool _isCloud;

    protected BaseMongoDbRepository(string connectionString, string databaseName, bool isCloud = false)
    {
        _connectionString = connectionString;
        _databaseName = databaseName;
        _isCloud = isCloud;
    }

    protected IMongoCollection<BsonDocument> GetCollection(string collectionName)
    {
        MongoClient client;

        if (_isCloud)
        {
            var settings = MongoClientSettings.FromConnectionString(_connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            client = new MongoClient(settings);
        }
        else
        {
            client = new MongoClient(_connectionString);
        }

        var database = client.GetDatabase(_databaseName);
        var collection = database.GetCollection<BsonDocument>(collectionName);
        return collection;
    }
}