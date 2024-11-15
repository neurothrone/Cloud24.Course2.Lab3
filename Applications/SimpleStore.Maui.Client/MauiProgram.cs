using Microsoft.Extensions.Logging;
using SimpleStore.Core.Interfaces;
using SimpleStore.Core.Services;
using SimpleStore.Maui.Client.Navigation;
using SimpleStore.Maui.Client.Services;
using SimpleStore.Maui.Client.ViewModels;
using SimpleStore.Maui.Client.Views.Pages;
using SimpleStore.Persistence.InMemory.Data;
using SimpleStore.Persistence.MongoDB.Repositories;
using SimpleStore.Persistence.Shared.Interfaces;

namespace SimpleStore.Maui.Client;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Font Awesome 6 Free-Solid-900.otf", "FaSolid");
            })
            .RegisterAppServices()
            .RegisterViewModels()
            .RegisterViews();

        // Register Routes
        Routing.RegisterRoute(nameof(AppRoute.Checkout), typeof(CheckoutPage));

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static MauiAppBuilder RegisterAppServices(this MauiAppBuilder builder)
    {
        // !: NOTE -> Authentication depends on ICustomerRepository
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

        // !: NOTE -> MongoDB Repository
        builder.Services.AddSingleton<ICustomerRepository, CustomerMongoDbRepository>(_ =>
            new CustomerMongoDbRepository("mongodb://localhost:27017", "SimpleStoreDB"));

        // !: NOTE -> Local File Repository
        // builder.Services.AddSingleton<ICustomerRepository, CustomerFileRepository>(provider =>
        // {
        //     var fileService = provider.GetRequiredService<IFileService>();
        //     // !: MacOS -> /Users/username/Library/Containers/com.companyname.simplestore.maui.client/Data/Library/customers.json
        //     var filePath = Path.Combine(FileSystem.AppDataDirectory, "customers.json");
        //     return new CustomerFileRepository(fileService, filePath);
        // });
        // builder.Services.AddSingleton<IFileService, FileService>();

        builder.Services.AddSingleton<IProductService, ProductService>();
        // !: NOTE -> MongoDB Repository
        builder.Services.AddSingleton<IProductRepository, ProductMongoDbRepository>(_ =>
            new ProductMongoDbRepository("mongodb://localhost:27017", "SimpleStoreDB"));
        // !: NOTE -> In-Memory Repository
        // builder.Services.AddSingleton<IProductRepository, InMemoryProductRepository>(_ =>
        // {
        //     var store = new InMemoryProductRepository();
        //     store.AddInitialData();
        //     return store;
        // });

        // !: NOTE -> Independent Services
        builder.Services.AddSingleton<IDialogService, DialogService>();
        builder.Services.AddSingleton<INavigator, Navigator>();
        builder.Services.AddSingleton<IUserPreferences, UserPreferences>();

        builder.Services.AddSingleton<AppState>();
        builder.Services.AddSingleton<CurrencyService>();

        return builder;
    }

    private static MauiAppBuilder RegisterViewModels(this MauiAppBuilder builder)
    {
        builder.Services.AddSingleton<AuthViewModel>();
        builder.Services.AddSingleton<ProductListViewModel>();

        builder.Services.AddTransient<CheckoutViewModel>();
        builder.Services.AddTransient<SettingsViewModel>();

        return builder;
    }

    // ReSharper disable once UnusedMethodReturnValue.Local
    private static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        builder.Services.AddTransient<ProductsPage>();
        builder.Services.AddTransient<CartPage>();
        builder.Services.AddTransient<CheckoutPage>();
        builder.Services.AddTransient<SettingsPage>();

        return builder;
    }
}