using SimpleStore.Core.Interfaces;

namespace SimpleStore.Maui.Client.Services;

public class DialogService : IDialogService
{
    public async Task ShowAlertAsync(string title, string message, string accept)
    {
        await MainThread.InvokeOnMainThreadAsync(
            () => Application.Current?.MainPage?.DisplayAlert(title, message, accept));
    }

    public async Task<bool> ShowPromptAsync(
        string title,
        string message,
        string accept,
        string cancel) => await MainThread.InvokeOnMainThreadAsync(
        () => Application.Current?.MainPage?.DisplayAlert(title, message, accept, cancel));
}