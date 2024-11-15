using SimpleStore.Persistence.FileStorage.Interfaces;
using SimpleStore.Persistence.FileStorage.Models;
using SimpleStore.Persistence.Shared.Interfaces;
using SimpleStore.Persistence.Shared.Utils;

namespace SimpleStore.Persistence.FileStorage.Repositories;

public class CustomerFileRepository : ICustomerRepository
{
    private readonly IFileService _fileService;
    private readonly string _filePath;

    public CustomerFileRepository(
        IFileService fileService,
        string filePath)
    {
        _fileService = fileService;
        _filePath = filePath;

        Initialize();
    }

    private void Initialize()
    {
        var customers = _fileService.ReadFromJsonFile<List<CustomerEntity>>(_filePath) ?? [];
        if (customers.Count != 0)
            return;

        WriteCustomersToFile(GetPredefinedCustomers());
    }

    private CustomerEntity[] GetPredefinedCustomers() =>
    [
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Username = "Knatte",
            Password = "123",
            TotalSpent = 0d
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Username = "Fnatte",
            Password = "321",
            TotalSpent = 50d
        },
        new()
        {
            Id = Guid.NewGuid().ToString("N"),
            Username = "Tjatte",
            Password = "213",
            TotalSpent = 100d
        }
    ];

    private void WriteCustomersToFile(ICollection<CustomerEntity> customers)
    {
        _ = _fileService.WriteToJsonFile(_filePath, customers);
    }

    #region ICustomerRepository

    public Task<Result<List<ICustomerEntity>>> RetrieveAllCustomersAsync()
    {
        try
        {
            var customers = _fileService.ReadFromJsonFile<List<CustomerEntity>>(_filePath) ?? [];
            return Task.FromResult(Result<List<ICustomerEntity>>.Success([..customers]));
        }
        catch (Exception ex)
        {
            return Task.FromResult(Result<List<ICustomerEntity>>.Failure(ex.Message));
        }
    }

    public async Task<Result<ICustomerEntity?>> RetrieveCustomerByIdAsync(string customerId)
    {
        var result = await RetrieveAllCustomersAsync();
        return result.Match(
            onSuccess: customers =>
            {
                var customer = customers.FirstOrDefault(c => c.Id == customerId);
                return Result<ICustomerEntity?>.Success(customer);
            },
            onFailure: Result<ICustomerEntity?>.Failure
        );
    }

    public async Task<Result<ICustomerEntity>> CreateCustomerAsync(string username, string password)
    {
        var result = await RetrieveAllCustomersAsync();
        return result.Match(
            onSuccess: customers =>
            {
                var newCustomer = new CustomerEntity
                {
                    Id = Guid.NewGuid().ToString("N"),
                    Username = username,
                    Password = password,
                    TotalSpent = 0d
                };
                customers.Add(newCustomer);

                try
                {
                    List<CustomerEntity> customersToWrite = customers
                        .Select(c => new CustomerEntity
                        {
                            Id = c.Id,
                            Username = c.Username,
                            Password = c.Password,
                            TotalSpent = c.TotalSpent
                        })
                        .ToList();
                    WriteCustomersToFile(customersToWrite);

                    return Result<ICustomerEntity>.Success(newCustomer);
                }
                catch (Exception ex)
                {
                    return Result<ICustomerEntity>.Failure(ex.Message);
                }
            },
            onFailure: Result<ICustomerEntity>.Failure
        );
    }

    public async Task<Result<bool>> UpdateCustomerAsync(string customerId, double totalSpent)
    {
        var result = await RetrieveAllCustomersAsync();
        return result.Match(
            onSuccess: customers =>
            {
                var customerToUpdate = customers.FirstOrDefault(c => c.Id == customerId);
                if (customerToUpdate is null)
                    return Result<bool>.Failure("Customer not found.");

                var updatedCustomer = new CustomerEntity
                {
                    Id = customerToUpdate.Id,
                    Username = customerToUpdate.Username,
                    Password = customerToUpdate.Password,
                    TotalSpent = totalSpent
                };

                try
                {
                    var index = customers.IndexOf(customerToUpdate);
                    customers[index] = updatedCustomer;

                    List<CustomerEntity> customersToWrite = customers
                        .Select(c => new CustomerEntity
                        {
                            Id = c.Id,
                            Username = c.Username,
                            Password = c.Password,
                            TotalSpent = c.TotalSpent
                        })
                        .ToList();
                    _ = _fileService.WriteToJsonFile(_filePath, new List<CustomerEntity>(customersToWrite));

                    return Result<bool>.Success(true);
                }
                catch (Exception ex)
                {
                    return Result<bool>.Failure(ex.Message);
                }
            },
            onFailure: Result<bool>.Failure
        );
    }

    public async Task<Result<bool>> DeleteCustomerAsync(string customerId)
    {
        var result = await RetrieveAllCustomersAsync();
        return result.Match(
            onSuccess: customers =>
            {
                var customerToDelete = customers.FirstOrDefault(c => c.Id == customerId);
                if (customerToDelete is null)
                    return Result<bool>.Failure("Customer not found.");

                try
                {
                    var deleted = customers.Remove(customerToDelete);
                    _ = _fileService.WriteToJsonFile(_filePath, customers);
                    return Result<bool>.Success(deleted);
                }
                catch (Exception ex)
                {
                    return Result<bool>.Failure(ex.Message);
                }
            },
            onFailure: Result<bool>.Failure
        );
    }

    #endregion
}