using SimpleStore.Core.Enums;

namespace SimpleStore.Core.Utils;

public static class CurrencyExtensions
{
    public static string Formatted(this Currency currency) => currency switch
    {
        Currency.Sek => "SEK",
        Currency.Usd => "USD",
        Currency.Eur => "EUR",
        _ => throw new NotSupportedException($"Currency {currency} is not supported")
    };
}