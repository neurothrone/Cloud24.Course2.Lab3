using SimpleStore.Maui.Client.ViewModels;

namespace SimpleStore.Maui.Client.Views.Pages;

public partial class CheckoutPage : ContentPage
{
    public CheckoutPage(CheckoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}