namespace SimpleStore.Maui.Client.Views.Controls;

public partial class CustomEntry : Entry
{
    public CustomEntry()
    {
        InitializeComponent();

        Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeNone);
    }
}