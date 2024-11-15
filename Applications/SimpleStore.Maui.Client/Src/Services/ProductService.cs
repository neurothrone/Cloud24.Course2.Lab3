using SimpleStore.Core.Interfaces;
using SimpleStore.Core.Models;
using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Maui.Client.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product[]> GetAvailableProductsAsync()
    {
        var result = await _productRepository.RetrieveAllProductsAsync();
        return result.Match(
            onSuccess: products => products
                .Select(entity => new Product
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Price = entity.Price,
                    Quantity = entity.Quantity
                })
                .ToArray(),
            onFailure: _ => []
        );
    }
}