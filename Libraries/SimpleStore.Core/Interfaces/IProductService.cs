using SimpleStore.Core.Models;

namespace SimpleStore.Core.Interfaces;

public interface IProductService
{
    Task<Product[]> GetAvailableProductsAsync();
}