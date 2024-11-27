using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SimpleStore.Core.Enums;
using SimpleStore.Core.Interfaces;
using SimpleStore.Core.Services;
using SimpleStore.Core.Utils;
using SimpleStore.Core.ViewModels;
using SimpleStore.Maui.Client.Messages;
using SimpleStore.Maui.Client.Services;
using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Maui.Client.ViewModels;

public class SettingsViewModel :
    ViewModel,
    IRecipient<CheckoutCompletedMessage>
{
    private readonly IUserPreferences _userPreferences;
    private readonly IDialogService _dialogService;
    private readonly AppState _appState;
    private readonly CurrencyService _currencyService;
    private readonly ICustomerRepository _customerRepository;

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

    public ICommand DeleteAccountCommand { get; }

    public SettingsViewModel(
        IUserPreferences userPreferences,
        IDialogService dialogService,
        ICustomerRepository customerRepository,
        AppState appState,
        CurrencyService currencyService)
    {
        _userPreferences = userPreferences;
        _dialogService = dialogService;
        _customerRepository = customerRepository;
        _appState = appState;
        _currencyService = currencyService;

        DeleteAccountCommand = new AsyncRelayCommand(DeleteAccount);

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

    private async Task DeleteAccount()
    {
        if (_appState.Customer is null)
            return;

        var shouldDelete = await _dialogService.ShowPromptAsync(
            title: "Delete Account",
            message: "Are you sure you want to delete your account?",
            accept: "Delete",
            cancel: "Cancel"
        );

        if (!shouldDelete)
            return;

        await _customerRepository.DeleteCustomerAsync(_appState.Customer.Id);
        SelectedCurrency = Currency.Sek; // Reset to SEK
        WeakReferenceMessenger.Default.Send<AccountDeletedMessage>();
    }

    #region IRecipient<CheckoutCompletedMessage>

    public void Receive(CheckoutCompletedMessage message)
    {
        UpdateTotalExpenditure();
    }

    #endregion
}