using SimpleStore.Core.Interfaces;

namespace SimpleStore.Maui.Client.Services;

public class DialogService : IDialogService
{
    public async Task ShowAlertAsync(string title, string message, string accept)
    {
        await MainThread.InvokeOnMainThreadAsync(
            () => Application.Current?.MainPage?.DisplayAlert(title, message, accept));
    }
}