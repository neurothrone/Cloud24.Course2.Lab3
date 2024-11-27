using SimpleStore.Core.Models;

namespace SimpleStore.Core.Interfaces;

public interface IAuthenticationService
{
    Task InitializeAsync();
    Task<Customer?> SignInAsync(string username, string password);
    Task<Customer?> SignUpAsync(string username, string password);
    Task UpdateCustomerAsync(Customer customer);
    public void RemoveCustomer(string customerId);
}