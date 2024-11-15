using SimpleStore.Core.Enums;
using SimpleStore.Core.Models;
using SimpleStore.Core.Utils;

namespace SimpleStore.Core.Factories;

public static class CustomerFactory
{
    public static Customer CreateCustomer(
        string id,
        string username,
        string password,
        double totalSpent)
    {
        return totalSpent.ToMembership() switch
        {
            Membership.Gold => new GoldCustomer(id, username, password, totalSpent),
            Membership.Silver => new SilverCustomer(id, username, password, totalSpent),
            _ => new BronzeCustomer(id, username, password, totalSpent)
        };
    }

    public static Customer CreateCustomer(Customer customer)
    {
        return customer.TotalSpent.ToMembership() switch
        {
            Membership.Gold => new GoldCustomer(customer.Id, customer.Username, customer.Password, customer.TotalSpent),
            Membership.Silver => new SilverCustomer(customer.Id, customer.Username, customer.Password,
                customer.TotalSpent),
            _ => new BronzeCustomer(customer.Id, customer.Username, customer.Password, customer.TotalSpent)
        };
    }
}