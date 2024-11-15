using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Messaging;
using SimpleStore.Core.Enums;
using SimpleStore.Core.Interfaces;
using SimpleStore.Core.Services;
using SimpleStore.Core.Utils;
using SimpleStore.Core.ViewModels;
using SimpleStore.Maui.Client.Messages;
using SimpleStore.Maui.Client.Services;

namespace SimpleStore.Maui.Client.ViewModels;

public class SettingsViewModel :
    ViewModel,
    IRecipient<CheckoutCompletedMessage>
{
    private readonly IUserPreferences _userPreferences;
    private readonly AppState _appState;
    private readonly CurrencyService _currencyService;

    public ObservableCollection<Currency> Currencies { get; } = [];
    private Currency _selectedCurrency;

    public string CurrencyFormat => _appState.Currency.Formatted();

    public double TotalExpenditure { get; set; }

    public Currency SelectedCurrency
    {
        get => _selectedCurrency;
        set
        {
            _appState.Currency = value;

            if (_selectedCurrency == value)
                return;

            _selectedCurrency = value;
            OnPropertyChanged();
            UpdateTotalExpenditure();
            _userPreferences.SaveCurrency(_selectedCurrency);

            WeakReferenceMessenger.Default.Send(new CurrencyChangedMessage(_selectedCurrency));
        }
    }

    public SettingsViewModel(
        IUserPreferences userPreferences,
        AppState appState,
        CurrencyService currencyService)
    {
        _userPreferences = userPreferences;
        _appState = appState;
        _currencyService = currencyService;

        WeakReferenceMessenger.Default.Register(this);

        foreach (Currency currency in Enum.GetValues(typeof(Currency)))
        {
            Currencies.Add(currency);
        }
    }

    protected override void Initialize()
    {
        MainThread.BeginInvokeOnMainThread(() =>
        {
            SelectedCurrency = _userPreferences.LoadCurrency();
            UpdateTotalExpenditure();
        });
    }

    private void UpdateTotalExpenditure()
    {
        if (_appState.Customer is null)
            return;

        TotalExpenditure = _currencyService.Convert(
            _appState.Customer.TotalSpent,
            Currency.Sek,
            _appState.Currency
        );

        OnPropertyChanged(nameof(TotalExpenditure));
        OnPropertyChanged(nameof(CurrencyFormat));
    }

    #region IRecipient<CheckoutCompletedMessage>

    public void Receive(CheckoutCompletedMessage message)
    {
        UpdateTotalExpenditure();
    }

    #endregion
}