namespace SimpleStore.Core.Interfaces;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string accept);
}