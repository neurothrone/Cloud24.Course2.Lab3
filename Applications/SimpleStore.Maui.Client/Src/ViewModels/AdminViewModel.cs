using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SimpleStore.Core.Enums;
using SimpleStore.Core.Models;
using SimpleStore.Core.Services;
using SimpleStore.Core.Utils;
using SimpleStore.Maui.Client.Messages;
using SimpleStore.Maui.Client.Services;
using SimpleStore.Persistence.MongoDB.Models;
using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Maui.Client.ViewModels;

public partial class AdminViewModel :
    ObservableObject,
    IRecipient<CurrencyChangedMessage>
{
    private readonly IProductRepository _productRepository;
    private readonly AppState _appState;
    private readonly CurrencyService _currencyService;

    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private double _price;

    [ObservableProperty]
    private int _quantity = 1;

    public ObservableCollection<StoreProductViewModel> Products { get; } = [];

    public string CurrencyFormat => _appState.Currency.Formatted();

    public AdminViewModel(
        IProductRepository productRepository,
        AppState appState,
        CurrencyService currencyService)
    {
        _productRepository = productRepository;
        _appState = appState;
        _currencyService = currencyService;

        WeakReferenceMessenger.Default.Register(this);
    }

    public async void OnAppearing()
    {
        var result = await _productRepository.RetrieveAllProductsAsync();
        result.When(
            onSuccess: products => MainThread.BeginInvokeOnMainThread(() => PopulateProducts(products)),
            onFailure: _ => { }
        );
    }

    private void PopulateProducts(List<IProductEntity> products)
    {
        Products.Clear();

        var viewModels = products
            .Select(p => new StoreProductViewModel(
                new Product
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = _currencyService.Convert(p.Price, Currency.Sek, _appState.Currency),
                    Quantity = p.Quantity
                }))
            .ToArray();

        MainThread.BeginInvokeOnMainThread(() =>
        {
            foreach (var productViewModel in viewModels)
            {
                Products.Add(productViewModel);
            }
        });
    }

    private async Task UpdateAllProductsCurrency(Currency currency)
    {
        _appState.Currency = currency;
        OnPropertyChanged(nameof(CurrencyFormat));

        var result = await _productRepository.RetrieveAllProductsAsync();
        result.When(
            onSuccess: originalProducts =>
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    foreach (var product in Products)
                    {
                        var originalProduct = originalProducts.FirstOrDefault(p => p.Name.Equals(product.Name));
                        product.Price = _currencyService.Convert(
                            originalProduct?.Price ?? product.Price,
                            Currency.Sek,
                            _appState.Currency);
                    }
                });
            },
            onFailure: _ => { }
        );
    }

    [RelayCommand]
    private async Task AddProduct()
    {
        var result = await _productRepository.CreateProductAsync(new ProductEntity
        {
            Name = Name,
            Price = Price,
            Quantity = Quantity
        });
        result.When(
            onSuccess: product =>
            {
                var productViewModel = new StoreProductViewModel(new Product
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity
                });
                MainThread.BeginInvokeOnMainThread(() => Products.Add(productViewModel));
                WeakReferenceMessenger.Default.Send(new ProductAddedMessage(productViewModel));
            },
            onFailure: _ => { }
        );
    }

    [RelayCommand]
    private async Task RemoveProduct(StoreProductViewModel product)
    {
        var result = await _productRepository.DeleteProductAsync(product.Id);
        result.When(
            onSuccess: _ =>
            {
                MainThread.BeginInvokeOnMainThread(() => Products.Remove(product));
                WeakReferenceMessenger.Default.Send(new ProductRemovedMessage(product.Id));
            },
            onFailure: error => { Console.WriteLine($"⚠️ -> Error: {error}"); }
        );
    }

    #region IRecipient<CurrencyChangedMessage>

    public async void Receive(CurrencyChangedMessage message)
    {
        await UpdateAllProductsCurrency(message.Currency);
    }

    #endregion
}