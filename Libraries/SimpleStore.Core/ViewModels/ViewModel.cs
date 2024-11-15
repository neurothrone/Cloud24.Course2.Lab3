using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SimpleStore.Core.ViewModels;

public abstract class ViewModel : INotifyPropertyChanged
{
    private bool _isInitialized = true;

    public void OnAppearing()
    {
        if (!_isInitialized)
            return;

        _isInitialized = false;
        Initialize();
    }

    protected virtual void Initialize()
    {
    }

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    #endregion
}