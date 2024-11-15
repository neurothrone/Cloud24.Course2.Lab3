using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SimpleStore.Core.Enums;
using SimpleStore.Core.Interfaces;
using SimpleStore.Core.Models;
using SimpleStore.Core.Services;
using SimpleStore.Core.Utils;
using SimpleStore.Core.ViewModels;
using SimpleStore.Maui.Client.Messages;
using SimpleStore.Maui.Client.Services;

namespace SimpleStore.Maui.Client.ViewModels;

public class CheckoutViewModel :
    ViewModel,
    IQueryAttributable
{
    private readonly INavigator _navigator;
    private readonly AppState _appState;
    private readonly CurrencyService _currencyService;

    private double _totalPrice;

    public double TotalPrice
    {
        get => _totalPrice;
        set
        {
            if (_totalPrice.Equals(value))
                return;

            _totalPrice = value;
            OnPropertyChanged();
        }
    }

    public string CurrencyFormat => _appState.Currency.Formatted();
    public string Membership => _appState.Customer?.Membership.ToString() ?? string.Empty;
    public string DiscountPercentage => $"{_appState.Customer?.Membership.Discount():P2}";

    public ObservableCollection<ProductViewModel> Products { get; } = [];

    public ICommand OrderCommand { get; }

    public CheckoutViewModel(
        INavigator navigator,
        AppState appState,
        CurrencyService currencyService)
    {
        _navigator = navigator;
        _appState = appState;
        _currencyService = currencyService;

        OrderCommand = new AsyncRelayCommand(Order);
    }

    private async Task Order()
    {
        var totalPriceInSek = _currencyService.Convert(TotalPrice, _appState.Currency, Currency.Sek);
        _appState.UpdateCustomerExpenditure(totalPriceInSek);

        WeakReferenceMessenger.Default.Send(new CheckoutCompletedMessage());
        await _navigator.GoBackAsync();
    }

    private void CalculateTotalPrice()
    {
        var sum = Products.Sum(p => p.Price * p.Quantity);

        if (_appState.Customer is not null)
            sum *= 1d - _appState.Customer.Membership.Discount();

        TotalPrice = sum;
    }

    private void JsonToProducts(string jsonProducts)
    {
        var products = JsonSerializer.Deserialize<Product[]>(jsonProducts) ?? [];

        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var product in products)
            {
                Products.Add(new ProductViewModel(product));
            }

            if (Products.Count != 0)
                CalculateTotalPrice();
        });
    }

    #region IQueryAttributable

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue("products", out var jsonValue) || jsonValue is not string json)
            return;

        JsonToProducts(json);
    }

    #endregion
}