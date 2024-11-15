using SimpleStore.Maui.Client.ViewModels;
using SimpleStore.Maui.Client.Views.Pages;

namespace SimpleStore.Maui.Client;

public partial class App : Application
{
    private readonly AuthViewModel _authViewModel;

    public App(AuthViewModel viewModel)
    {
        InitializeComponent();

        // Force light theme
        if (Current is not null)
            Current.UserAppTheme = AppTheme.Light;

        _authViewModel = viewModel;
        _authViewModel.AuthStateChanged += OnAuthStateChanged;
        OnAuthStateChanged(_authViewModel.IsAuthenticated);
    }

    private void OnAuthStateChanged(bool isAuthenticated)
    {
        // Change the MainPage based on the authentication state
        MainPage = isAuthenticated ? new AppShell() : new AuthPage(_authViewModel);
    }
}