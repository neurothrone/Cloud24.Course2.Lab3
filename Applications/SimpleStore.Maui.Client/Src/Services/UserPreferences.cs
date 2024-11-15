using SimpleStore.Core.Enums;
using SimpleStore.Core.Interfaces;

namespace SimpleStore.Maui.Client.Services;

public class UserPreferences : IUserPreferences
{
    public Currency LoadCurrency()
    {
        var savedCurrency = Preferences.Default.Get(nameof(Currency), Currency.Sek.ToString());
        return Enum.TryParse(savedCurrency, out Currency result) ? result : Currency.Sek;
    }

    public void SaveCurrency(Currency currency)
    {
        Preferences.Default.Set(nameof(Currency), currency.ToString());
    }
}