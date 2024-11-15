using SimpleStore.Core.Factories;
using SimpleStore.Core.Models;
using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Core.Utils;

public static class CustomerExtensions
{
    public static Customer ToCustomer(this ICustomerEntity entity) => CustomerFactory.CreateCustomer(
        entity.Id,
        entity.Username,
        entity.Password,
        entity.TotalSpent
    );
}