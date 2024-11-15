using SimpleStore.Maui.Client.ViewModels;

namespace SimpleStore.Maui.Client.Views.Pages;

public partial class ProductsPage : ContentPage
{
    public ProductsPage(ProductListViewModel productListViewModel)
    {
        InitializeComponent();
        BindingContext = productListViewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is ProductListViewModel viewModel)
            viewModel.OnAppearing();
    }
}