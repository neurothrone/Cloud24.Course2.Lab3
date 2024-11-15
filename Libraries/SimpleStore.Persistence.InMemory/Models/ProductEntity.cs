using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Persistence.InMemory.Models;

public class ProductEntity : IProductEntity
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public int Quantity { get; set; }
}