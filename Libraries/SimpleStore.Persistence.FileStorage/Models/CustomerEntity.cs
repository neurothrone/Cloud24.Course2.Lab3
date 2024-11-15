using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Persistence.FileStorage.Models;

public class CustomerEntity : ICustomerEntity
{
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public double TotalSpent { get; set; }
};