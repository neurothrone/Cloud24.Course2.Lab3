using SimpleStore.Core.Models;

namespace SimpleStore.Maui.Client.ViewModels;

public class StoreProductViewModel : ProductViewModel
{
    private readonly int _maxQuantity;

    public override int Quantity
    {
        get => base.Quantity;
        set
        {
            base.Quantity = value;
            OnPropertyChanged(nameof(QuantityStatus));
        }
    }

    public StoreProductViewModel(Product product) : base(product)
    {
        _maxQuantity = product.Quantity;
    }

    public string QuantityStatus => $"{Quantity} / {_maxQuantity}";
}