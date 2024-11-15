using SimpleStore.Core.Enums;

namespace SimpleStore.Core.Interfaces;

public interface IUserPreferences
{
    Currency LoadCurrency();
    void SaveCurrency(Currency currency);
}