using SimpleStore.Persistence.Shared.Utils;

namespace SimpleStore.Persistence.Shared.Interfaces;

public interface IProductRepository
{
    Task<Result<List<IProductEntity>>> RetrieveAllProductsAsync();
    Task<Result<IProductEntity?>> RetrieveProductByIdAsync(string productId);
    Task<Result<IProductEntity>> CreateProductAsync(IProductEntity product);
    Task<Result<bool>> UpdateProductAsync(IProductEntity product);
    Task<Result<bool>> DeleteProductAsync(string productId);
}