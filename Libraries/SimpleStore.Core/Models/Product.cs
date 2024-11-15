namespace SimpleStore.Core.Models;

public class Product
{
    public string Id { get; set; } = string.Empty;
    public required string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
}