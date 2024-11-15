using SimpleStore.Core.Enums;

namespace SimpleStore.Core.Services;

public class CurrencyService
{
    private const double SekToUsdConversionRate = 0.098;
    private const double SekToEurConversionRate = 0.088;

    private const double UsdToSekConversionRate = 10.24;
    private const double UsdToEurConversionRate = 0.090;

    private const double EurToSekConversionRate = 11.36;
    private const double EurToUsdConversionRate = 1.11;

    public double Convert(double amount, Currency from, Currency to)
    {
        if (from.Equals(to))
            return amount;

        return from switch
        {
            Currency.Sek when to == Currency.Usd => amount * SekToUsdConversionRate,
            Currency.Sek when to == Currency.Eur => amount * SekToEurConversionRate,
            Currency.Usd when to == Currency.Sek => amount * UsdToSekConversionRate,
            Currency.Usd when to == Currency.Eur => amount * UsdToEurConversionRate,
            Currency.Eur when to == Currency.Sek => amount * EurToSekConversionRate,
            Currency.Eur when to == Currency.Usd => amount * EurToUsdConversionRate,
            _ => amount
        };
    }
}