using SimpleStore.Core.Models;
using SimpleStore.Core.ViewModels;

namespace SimpleStore.Maui.Client.ViewModels;

public class ProductViewModel : ViewModel
{
    public Product Product { get; }

    public string Name
    {
        get => Product.Name;
        set
        {
            if (Product.Name.Equals(value))
                return;

            Product.Name = value;
            OnPropertyChanged();
        }
    }

    public double Price
    {
        get => Product.Price;
        set
        {
            if (Product.Price.Equals(value))
                return;

            Product.Price = value;
            OnPropertyChanged();
        }
    }

    public virtual int Quantity
    {
        get => Product.Quantity;
        set
        {
            if (Product.Quantity.Equals(value))
                return;

            Product.Quantity = value;
            OnPropertyChanged();
        }
    }

    public double TotalPrice => Price * Quantity;

    public override string ToString()
    {
        return $"{Name} - {Price:0.00} * {Quantity}x : {TotalPrice:0.00}";
    }

    public ProductViewModel(Product product)
    {
        Product = product;
    }
}