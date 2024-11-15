using SimpleStore.Core.Enums;

namespace SimpleStore.Core.Utils;

public static class MembershipExtensions
{
    public static Membership ToMembership(this double totalSpent) => totalSpent switch
    {
        >= 100 => Membership.Gold,
        >= 50 => Membership.Silver,
        _ => Membership.Bronze
    };

    public static double Discount(this Membership membership) => membership switch
    {
        Membership.Gold => 0.15,
        Membership.Silver => 0.10,
        Membership.Bronze => 0.05,
        _ => 0d
    };
}