using SimpleStore.Persistence.InMemory.Models;
using SimpleStore.Persistence.Shared.Interfaces;
using SimpleStore.Persistence.Shared.Utils;

namespace SimpleStore.Persistence.InMemory.Data;

public class InMemoryProductRepository : IProductRepository
{
    private readonly List<IProductEntity> _products = [];

    public void AddInitialData()
    {
        List<IProductEntity> products =
        [
            new ProductEntity
            {
                Name = "Apples, 1kg",
                Price = 15,
                Quantity = 20
            },
            new ProductEntity
            {
                Name = "Bananas, 1kg",
                Price = 30,
                Quantity = 10
            },
            new ProductEntity
            {
                Name = "Milk, 1L",
                Price = 22,
                Quantity = 15
            }
        ];

        foreach (var product in products)
            CreateProductAsync(product);
    }

    #region IDatabase

    public Task<Result<List<IProductEntity>>> RetrieveAllProductsAsync()
    {
        return Task.FromResult(Result<List<IProductEntity>>.Success(_products));
    }

    public Task<Result<IProductEntity?>> RetrieveProductByIdAsync(string productId)
    {
        return Task.FromResult(Result<IProductEntity?>.Success(_products.FirstOrDefault(p => p.Id == productId)));
    }

    public Task<Result<IProductEntity>> CreateProductAsync(IProductEntity product)
    {
        product.Id = Guid.NewGuid().ToString("N");
        _products.Add(product);
        return Task.FromResult(Result<IProductEntity>.Success(product));
    }

    public Task<Result<bool>> UpdateProductAsync(IProductEntity product)
    {
        var productToUpdate = _products.FirstOrDefault(p => p.Id == product.Id);
        if (productToUpdate is null)
            return Task.FromResult(Result<bool>.Success(false));

        productToUpdate.Name = product.Name;
        productToUpdate.Price = product.Price;
        productToUpdate.Quantity = product.Quantity;

        return Task.FromResult(Result<bool>.Success(true));
    }

    public Task<Result<bool>> DeleteProductAsync(string productId)
    {
        var productToDelete = _products.FirstOrDefault(p => p.Id == productId);
        return Task.FromResult(productToDelete is null
            ? Result<bool>.Success(false)
            : Result<bool>.Success(_products.Remove(productToDelete)));
    }

    #endregion
}