using System.Net;
using Microsoft.Azure.Cosmos;
using SimpleStore.Persistence.Shared.Interfaces;
using SimpleStore.Persistence.Shared.Utils;

namespace SimpleStore.Persistence.Cosmos.Repositories;

public class ProductCosmosDbRepository : IProductRepository
{
    // !: Connection string
    private const string ConnectionString =
        "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";

    // !: Endpoint and key
    private const string Endpoint = "https://dp420.documents.azure.com:443/";

    private const string Key =
        "C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==";


    public ProductCosmosDbRepository()
    {
        CosmosClientOptions options = new()
        {
            LimitToEndpoint = true
        };
        CosmosClient client = new(Endpoint, Key, options);

        // CosmosClient client = new(ConnectionString);
        // CosmosClient client = new(Endpoint, Key);
    }


    public async Task<Result<List<IProductEntity>>> RetrieveAllProductsAsync()
    {
        using var client = new CosmosClient(
            "endpoint",
            "key"
        );

        Database database = await client.CreateDatabaseIfNotExistsAsync("SimpleStoreDB");
        Container container = await database.CreateContainerIfNotExistsAsync(
            id: "products",
            partitionKeyPath: "/product_id",
            throughput: 400
        );

        // await container.GetItemLinqQueryable<IProductEntity>();
        
        throw new NotImplementedException();
    }

    public async Task<Result<IProductEntity?>> RetrieveProductByIdAsync(string productId)
    {
        using var client = new CosmosClient(
            "endpoint",
            "key"
        );

        Database database = await client.CreateDatabaseIfNotExistsAsync("SimpleStoreDB");
        Container container = await database.CreateContainerIfNotExistsAsync(
            id: "products",
            partitionKeyPath: "/product_id",
            throughput: 400
        );

        var response = await container.ReadItemAsync<IProductEntity>(productId, new PartitionKey(productId));

        throw new NotImplementedException();
    }

    public async Task<Result<IProductEntity>> CreateProductAsync(IProductEntity product)
    {
        using var client = new CosmosClient(
            "endpoint",
            "key"
        );

        Database database = await client.CreateDatabaseIfNotExistsAsync("SimpleStoreDB");
        Container container = await database.CreateContainerIfNotExistsAsync(
            id: "products",
            partitionKeyPath: "/product_id",
            throughput: 400
        );

        // TODO: new ProductEntity, upsert.
        try
        {
            ItemResponse<IProductEntity> response = await container.UpsertItemAsync(product);
            if (response.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created)
            {
                return Result<IProductEntity>.Success(response.Resource);
            }

            return Result<IProductEntity>.Failure("Failed to create product.");
        }
        catch (CosmosException ex) when (ex.StatusCode == HttpStatusCode.Conflict)
        {
            // Add logic to handle conflicting ids
            return Result<IProductEntity>.Failure(ex.Message);
        }
        catch (CosmosException ex)
        {
            return Result<IProductEntity>.Failure(ex.Message);
        }
    }

    public async Task<Result<bool>> UpdateProductAsync(IProductEntity product)
    {
        using var client = new CosmosClient(
            "endpoint",
            "key"
        );

        Database database = await client.CreateDatabaseIfNotExistsAsync("SimpleStoreDB");
        Container container = await database.CreateContainerIfNotExistsAsync(
            id: "products",
            partitionKeyPath: "/product_id",
            throughput: 400
        );

        // var response = await container.ReplaceItemAsync<IProductEntity>(product);
        var response = await container.ReplaceItemAsync<IProductEntity>(product, product.Id);
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> DeleteProductAsync(string productId)
    {
        using var client = new CosmosClient(
            "endpoint",
            "key"
        );

        Database database = await client.CreateDatabaseIfNotExistsAsync("SimpleStoreDB");
        Container container = await database.CreateContainerIfNotExistsAsync(
            id: "products",
            partitionKeyPath: "/product_id",
            throughput: 400
        );

        var response = container.DeleteItemAsync<IProductEntity>(productId, new PartitionKey(productId));

        throw new NotImplementedException();
    }
}