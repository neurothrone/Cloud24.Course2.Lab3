using SimpleStore.Core.Enums;

namespace SimpleStore.Core.Models;

public class SilverCustomer : Customer
{
    public SilverCustomer(
        string id,
        string username,
        string password,
        double totalSpent) : base(id, username, password, totalSpent)
    {
        Membership = Membership.Silver;
    }
}