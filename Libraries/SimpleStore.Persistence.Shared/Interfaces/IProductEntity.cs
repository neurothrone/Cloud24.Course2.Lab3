namespace SimpleStore.Persistence.Shared.Interfaces;

public interface IProductEntity
{
    string Id { get; set; }
    string Name { get; set; }
    double Price { get; set; }
    int Quantity { get; set; }
}