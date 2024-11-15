namespace SimpleStore.Persistence.Shared.Interfaces;

public interface ICustomerEntity
{
    string Id { get; }
    string Username { get; set; }
    string Password { get; set; }
    double TotalSpent { get; set; }
}