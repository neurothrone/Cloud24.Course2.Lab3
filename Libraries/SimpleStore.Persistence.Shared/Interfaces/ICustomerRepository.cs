using SimpleStore.Persistence.Shared.Utils;

namespace SimpleStore.Persistence.Shared.Interfaces;

public interface ICustomerRepository
{
    Task<Result<List<ICustomerEntity>>> RetrieveAllCustomersAsync();
    Task<Result<ICustomerEntity?>> RetrieveCustomerByIdAsync(string customerId);
    Task<Result<ICustomerEntity>> CreateCustomerAsync(string username, string password);
    Task<Result<bool>> UpdateCustomerAsync(string customerId, double totalSpent);
    Task<Result<bool>> DeleteCustomerAsync(string customerId);
}