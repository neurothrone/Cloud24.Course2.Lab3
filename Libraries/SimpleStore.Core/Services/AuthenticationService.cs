using SimpleStore.Core.Factories;
using SimpleStore.Core.Interfaces;
using SimpleStore.Core.Models;
using SimpleStore.Core.Utils;
using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Core.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly List<Customer> _customers = [];

    public AuthenticationService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    private async Task LoadCustomersAsync()
    {
        var result = await _customerRepository.RetrieveAllCustomersAsync();
        result.When(
            onSuccess: customers => _customers.AddRange(
                customers.Select(c => CustomerFactory.CreateCustomer(c.ToCustomer()))
            ),
            onFailure: _ => { }
        );
    }

    #region IAuthenticationService

    public async Task InitializeAsync()
    {
        await LoadCustomersAsync();
    }

    public Task<Customer?> SignInAsync(string username, string password)
    {
        return Task.FromResult(
            _customers.FirstOrDefault(c => c.Username.Equals(username) && c.IsPasswordValid(password)));
    }

    public async Task<Customer?> SignUpAsync(string username, string password)
    {
        if (_customers.Exists(c => c.Username.Equals(username, StringComparison.Ordinal)))
            return null;

        var result = await _customerRepository.CreateCustomerAsync(username, password);
        return result.Match<Customer?>(
            onSuccess: entity =>
            {
                var newCustomer = entity.ToCustomer();
                _customers.Add(newCustomer);
                return newCustomer;
            },
            onFailure: _ => null
        );
    }

    public async Task UpdateCustomerAsync(Customer customer)
    {
        Customer? existingCustomer =
            _customers.FirstOrDefault(c => c.Username.Equals(customer.Username, StringComparison.Ordinal));
        if (existingCustomer is null)
            return;

        var result = await _customerRepository.UpdateCustomerAsync(existingCustomer.Id, customer.TotalSpent);
        result.When(
            onSuccess: _ =>
            {
                var index = _customers.IndexOf(existingCustomer);
                _customers[index] = customer;
            },
            onFailure: _ => { }
        );
    }

    public void RemoveCustomer(string customerId)
    {
        var customer = _customers.FirstOrDefault(c => c.Id == customerId);
        if (customer is null)
            return;

        _customers.Remove(customer);
    }

    #endregion
}