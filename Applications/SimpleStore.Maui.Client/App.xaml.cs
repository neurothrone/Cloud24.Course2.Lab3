using System.Globalization;
using SimpleStore.Maui.Client.ViewModels;
using SimpleStore.Maui.Client.Views.Pages;

namespace SimpleStore.Maui.Client;

public partial class App : Application
{
    private readonly AuthViewModel _authViewModel;

    public App(AuthViewModel viewModel)
    {
        InitializeComponent();
        
        // Set the default culture to en-US
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("en-US");

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