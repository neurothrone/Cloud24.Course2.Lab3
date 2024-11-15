using MongoDB.Bson;
using MongoDB.Driver;
using SimpleStore.Persistence.MongoDB.Models;
using SimpleStore.Persistence.Shared.Interfaces;
using SimpleStore.Persistence.Shared.Utils;

namespace SimpleStore.Persistence.MongoDB.Repositories;

public class ProductMongoDbRepository : BaseMongoDbRepository, IProductRepository
{
    private const string CollectionName = "products";

    public ProductMongoDbRepository(string connectionString, string databaseName) : base(connectionString,
        databaseName)
    {
    }

    private static ProductEntity? ConvertBsonToProduct(BsonDocument document)
    {
        try
        {
            return new ProductEntity
            {
                Id = document["product_id"].AsString,
                Name = document["name"].AsString,
                Price = document["price"].AsDouble,
                Quantity = document["quantity"].AsInt32
            };
        }
        catch
        {
            return null;
        }
    }

    #region IProductRepository

    public async Task<Result<List<IProductEntity>>> RetrieveAllProductsAsync()
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var documents = await collection
                .Find(_ => true)
                .ToListAsync();

            if (documents == null || documents.Count == 0)
                return Result<List<IProductEntity>>.Success([]);

            var products = documents
                .Select(ConvertBsonToProduct)
                .OfType<IProductEntity>()
                .ToList();

            return Result<List<IProductEntity>>.Success(products);
        }
        catch (Exception ex)
        {
            return Result<List<IProductEntity>>.Failure(ex.Message);
        }
    }

    public async Task<Result<IProductEntity?>> RetrieveProductByIdAsync(string productId)
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("product_id", productId);

            IAsyncCursor<BsonDocument> cursor = await collection.FindAsync(filter);
            BsonDocument document = await cursor.FirstOrDefaultAsync();

            var product = ConvertBsonToProduct(document);
            return product is null
                ? Result<IProductEntity?>.Failure("Product not found.")
                : Result<IProductEntity?>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<IProductEntity?>.Failure(ex.Message);
        }
    }

    public async Task<Result<IProductEntity>> CreateProductAsync(IProductEntity product)
    {
        try
        {
            var collection = GetCollection(CollectionName);

            var document = new BsonDocument
            {
                { "product_id", Guid.NewGuid().ToString("N") }, // NOTE!: N omits the dashes
                { "name", product.Name },
                { "price", product.Price },
                { "quantity", product.Quantity }
            };

            await collection.InsertOneAsync(document);
            if (!document["_id"].IsObjectId)
                return Result<IProductEntity>.Failure("Failed to insert product.");

            var createdProduct = ConvertBsonToProduct(document);
            return createdProduct is null
                ? Result<IProductEntity>.Failure("Failed to create product.")
                : Result<IProductEntity>.Success(createdProduct);
        }
        catch (Exception ex)
        {
            return Result<IProductEntity>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateProductAsync(IProductEntity product)
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("customer_id", product.Id);
            var update = Builders<BsonDocument>.Update
                .Set("name", product.Name)
                .Set("price", product.Price)
                .Set("quantity", product.Quantity);

            UpdateResult result = await collection.UpdateOneAsync(filter, update);
            if (result.MatchedCount == 0)
                return Result<bool>.Failure("Product not found.");

            return result.ModifiedCount > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure("Failed to update product.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> DeleteProductAsync(string productId)
    {
        try
        {
            var collection = GetCollection(CollectionName);
            var filter = Builders<BsonDocument>.Filter.Eq("product_id", productId);

            DeleteResult result = await collection.DeleteOneAsync(filter);
            return result.DeletedCount > 0
                ? Result<bool>.Success(true)
                : Result<bool>.Failure("Product not found.");
        }
        catch (Exception ex)
        {
            return Result<bool>.Failure(ex.Message);
        }
    }

    #endregion
}