using System.ComponentModel;

namespace SimpleStore.Maui.Client.Views.Controls;

public class BindableToolbarItem : ToolbarItem, IDisposable
{
    public static readonly BindableProperty EnabledIconProperty =
        BindableProperty.Create(nameof(EnabledIcon), typeof(ImageSource), typeof(BindableToolbarItem));

    public ImageSource EnabledIcon
    {
        get => (ImageSource)GetValue(EnabledIconProperty);
        set => SetValue(EnabledIconProperty, value);
    }

    public static readonly BindableProperty DisabledIconProperty =
        BindableProperty.Create(nameof(DisabledIcon), typeof(ImageSource), typeof(BindableToolbarItem));

    public ImageSource DisabledIcon
    {
        get => (ImageSource)GetValue(DisabledIconProperty);
        set => SetValue(DisabledIconProperty, value);
    }

    public BindableToolbarItem()
    {
        PropertyChanged += OnToolbarItemPropertyChanged;
        IconImageSource = IsEnabled ? EnabledIcon : DisabledIcon;
    }

    private void OnToolbarItemPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == IsEnabledProperty.PropertyName)
        {
            IconImageSource = IsEnabled ? EnabledIcon : DisabledIcon;
        }
    }

    #region IDisposable

    public void Dispose()
    {
        PropertyChanged -= OnToolbarItemPropertyChanged;
    }

    #endregion
}