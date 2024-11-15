using SimpleStore.Maui.Client.ViewModels;

namespace SimpleStore.Maui.Client.Views.Pages;

public partial class CartPage : ContentPage
{
    public CartPage(ProductListViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}