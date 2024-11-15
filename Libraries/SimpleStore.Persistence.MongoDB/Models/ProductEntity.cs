using System.Text.Json.Serialization;
using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Persistence.MongoDB.Models;

public class ProductEntity : IProductEntity
{
    [JsonPropertyName("product_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }
}