namespace Bluebird.Pages;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        this.InitializeComponent();
        NavView.SelectedItem = NavView.MenuItems[0];
        NavigateToPage("General");
    }

    private void NavigationView_ItemInvoked(muxc.NavigationView sender, muxc.NavigationViewItemInvokedEventArgs args)
    {
        // Get invoked item
        object pageTag = args.InvokedItemContainer.Tag;
        // Navigate to selected page
        NavigateToPage(pageTag);
    }

    private void NavigateToPage(object page)
    {
        // Navigate to selected page
        contentFrame.Navigate(Type.GetType($"Bluebird.Pages.SettingPages.{page}"));
        // Set NavView header
        NavView.Header = page;
    }
}
