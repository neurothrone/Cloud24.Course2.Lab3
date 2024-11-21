using MongoDB.Bson;
using MongoDB.Driver;
using SimpleStore.Persistence.MongoDB.Models;
using SimpleStore.Persistence.Shared.Interfaces;
using SimpleStore.Persistence.Shared.Utils;

namespace SimpleStore.Persistence.MongoDB.Repositories;

public class CustomerMongoDbRepository : BaseMongoDbRepository, ICustomerRepository
{
    private const string CollectionName = "customers";

    public CustomerMongoDbRepository(
        string connectionString,
        string databaseName,
        bool isCloud = false) : base(connectionString, databaseName, isCloud)
    {
    }

    private static CustomerEntity? ConvertBsonToCustomer(BsonDocument document)
    {
        try
        {
            return new CustomerEntity
            {
                Id = document["customer_id"].AsString,
                Username = document["username"].AsString,
                Password = document["password"].AsString,
                TotalSpent = document["total_spent"].AsDouble
            };
        }
        catch
        {
            return null;
        }
    }

    #region ICustomerRepository

    public async Task<Result<List<ICustomerEntity>>> RetrieveAllCustomersAsync()
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var documents = await collection
                .Find(_ => true)
                .ToListAsync();

            if (documents == null || documents.Count == 0)
                return Result<List<ICustomerEntity>>.Success([]);

            var customers = documents
                .Select(ConvertBsonToCustomer)
                .OfType<ICustomerEntity>()
                .ToList();

            return Result<List<ICustomerEntity>>.Success(customers);
        }
        catch (Exception ex)
        {
            return Result<List<ICustomerEntity>>.Failure(ex.Message);
        }
    }

    public async Task<Result<ICustomerEntity?>> RetrieveCustomerByIdAsync(string customerId)
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("customer_id", customerId);

            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
            BsonDocument document = await cursor.FirstOrDefaultAsync();

            var customer = ConvertBsonToCustomer(document);
            return customer is null
                ? Result<ICustomerEntity?>.Failure("Customer not found.")
                : Result<ICustomerEntity?>.Success(customer);
        }
        catch (Exception ex)
        {
            return Result<ICustomerEntity?>.Failure(ex.Message);
        }
    }

    public async Task<Result<ICustomerEntity>> CreateCustomerAsync(string username, string password)
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var document = new BsonDocument
            {
                { "customer_id", Guid.NewGuid().ToString("N") }, // NOTE!: N omits the dashes
                { "username", username },
                { "password", password },
                { "total_spent", 0d }
            };

            await collection.InsertOneAsync(document);
            if (!document["_id"].IsObjectId)
                return Result<ICustomerEntity>.Failure("Failed to insert customer.");

            var createdCustomer = ConvertBsonToCustomer(document);
            return createdCustomer is null
                ? Result<ICustomerEntity>.Failure("Failed to create customer.")
                : Result<ICustomerEntity>.Success(createdCustomer);
        }
        catch (Exception ex)
        {
            return Result<ICustomerEntity>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateCustomerAsync(string customerId, double totalSpent)
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("customer_id", customerId);
            var update = Builders<BsonDocument>.Update
                .Set("total_spent", totalSpent);

            UpdateResult result = await collection.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 0)
                return Result<bool>.Failure("Customer not found.");

            return result.ModifiedCount > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure("Failed to update customer.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteCustomerAsync(string customerId)
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("customer_id", customerId);

            DeleteResult result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure("Customer not found.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    #endregion
}