using SimpleStore.Core.Enums;
using SimpleStore.Core.Factories;
using SimpleStore.Core.Interfaces;
using SimpleStore.Core.Models;

namespace SimpleStore.Maui.Client.Services;

public class AppState
{
    public Customer? Customer { get; set; }
    public Currency Currency { get; set; }

    public AppState(IUserPreferences userPreferences)
    {
        Currency = userPreferences.LoadCurrency();
    }

    public void UpdateCustomerExpenditure(double expenditureInSek)
    {
        if (Customer is null)
            return;

        Customer.TotalSpent += expenditureInSek;
        Customer = CustomerFactory.CreateCustomer(Customer);
    }
}