using System.Globalization;
using SimpleStore.Core.Enums;

namespace SimpleStore.Maui.Client.Converters;

public class CurrencyToStringConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not Currency currency)
            return value;

        return currency switch
        {
            Currency.Sek => "SEK - Swedish Krona",
            Currency.Usd => "USD - US Dollar",
            Currency.Eur => "EUR - Euro",
            _ => currency.ToString()
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}