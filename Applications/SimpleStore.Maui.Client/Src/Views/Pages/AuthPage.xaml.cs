using SimpleStore.Maui.Client.ViewModels;

namespace SimpleStore.Maui.Client.Views.Pages;

public partial class AuthPage : ContentPage
{
    public AuthPage(AuthViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is AuthViewModel viewModel)
            viewModel.OnAppearing();
    }
}