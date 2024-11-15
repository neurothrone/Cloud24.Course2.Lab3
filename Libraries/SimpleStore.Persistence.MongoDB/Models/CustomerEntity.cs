using System.Text.Json.Serialization;
using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Persistence.MongoDB.Models;

public class CustomerEntity : ICustomerEntity
{
    [JsonPropertyName("customer_id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("total_spent")]
    public double TotalSpent { get; set; }
}