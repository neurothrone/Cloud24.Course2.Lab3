using System.Net;
using Microsoft.Azure.Cosmos;
using SimpleStore.Persistence.Shared.Interfaces;
using SimpleStore.Persistence.Shared.Utils;

namespace SimpleStore.Persistence.Cosmos.Repositories;

public class ProductCosmosDbRepository : IProductRepository
{
    public async Task<Result<List<IProductEntity>>> RetrieveAllProductsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<Result<IProductEntity?>> RetrieveProductByIdAsync(string productId)
    {
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
        ItemResponse<IProductEntity> response = await container.UpsertItemAsync(product);
        if (response.StatusCode is HttpStatusCode.OK or HttpStatusCode.Created)
        {
            return Result<IProductEntity>.Success(product);
        }

        return Result<IProductEntity>.Failure("Failed to create product.");
    }

    public async Task<Result<bool>> UpdateProductAsync(IProductEntity product)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> DeleteProductAsync(string productId)
    {
        throw new NotImplementedException();
    }
}