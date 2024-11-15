namespace SimpleStore.Maui.Client;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
    }

    private void OnNavigatedHandle(object sender, ShellNavigatedEventArgs e)
    {
        // Continue only if the user switches to a different tab
        if (e.Source != ShellNavigationSource.ShellSectionChanged &&
            e.Source != ShellNavigationSource.ShellContentChanged)
            return;

        // If the current navigation stack is not at root, pop all pages except the root
        var stack = Current.Navigation.NavigationStack;
        while (stack.Count > 1)
            Current.Navigation.RemovePage(stack[stack.Count - 1]);
    }
}