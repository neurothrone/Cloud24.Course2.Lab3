using SimpleStore.Core.Enums;

namespace SimpleStore.Core.Models;

public class GoldCustomer : Customer
{
    public GoldCustomer(
        string id,
        string username,
        string password,
        double totalSpent) : base(id, username, password, totalSpent)
    {
        Membership = Membership.Gold;
    }
}