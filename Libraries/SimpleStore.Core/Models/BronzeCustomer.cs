using SimpleStore.Core.Enums;

namespace SimpleStore.Core.Models;

public class BronzeCustomer : Customer
{
    public BronzeCustomer(
        string id,
        string username,
        string password,
        double totalSpent) : base(id, username, password, totalSpent)
    {
        Membership = Membership.Bronze;
    }
}