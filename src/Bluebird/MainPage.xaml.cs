using Bluebird.Pages;
using Windows.UI;

namespace Bluebird;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        CustomTitleBar();
        DataContext = ViewModels.SettingsViewModel.SettingsVM;
    }

    private void CustomTitleBar()
    {
        // Hide default title bar.
        var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        coreTitleBar.ExtendViewIntoTitleBar = true;
        // Set custom XAML element as titlebar
        Window.Current.SetTitleBar(CustomDragRegion);
        var titleBar = ApplicationView.GetForCurrentView().TitleBar;
        // Set colors
        titleBar.ButtonBackgroundColor = Colors.Transparent;
    }

    private async void MoreFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        BrowserMenuFlyout.Hide();
        switch ((sender as AppBarButton).Tag)
        {
            case "NewTab":
                CreateWebTab();
                break;
            case "NewWindow":
                break;
            case "Downloads":
                launchurl = "edge://downloads";
                CreateTab("Downloads", "\uE896", typeof(WebViewPage));
                break;
            case "Favorites":
                CreateTab("Favorites", "\uE728", typeof(FavoritesPage));
                break;
            case "History":
                launchurl = "edge://history";
                CreateTab("History", "\uE81C", typeof(WebViewPage));
                break;
            case "Fullscreen":
                var view = ApplicationView.GetForCurrentView();
                if (!view.IsFullScreenMode)
                    WindowManager.EnterFullScreen(true);
                else
                    WindowManager.EnterFullScreen(false);
                break;
            case "Settings":
                CreateTab("Settings", "\uE115", typeof(SettingsPage));
                break;
            case "Help":
                // TODO: Add link to help site
                break;
            case "About":
                Dialogs.AboutContentDialog aboutDialog = new();
                var result = await aboutDialog.ShowAsync();
                if (result == ContentDialogResult.Primary)
                {
                    CreateTab("Attributions", "\uE946", typeof(AttributionsPage));
                }
                else if (result == ContentDialogResult.Secondary)
                {
                    launchurl = "https://github.com/bluebird-developers/browser";
                    CreateWebTab();
                }
                break;
        }
    }

    public muxc.TabViewItem SelectedTab
    {
        get
        {
            muxc.TabViewItem selectedItem = (muxc.TabViewItem)Tabs.SelectedItem;
            if (selectedItem != null)
                return selectedItem;
            return null;
        }
    }

    private void Tabs_Loaded(object sender, RoutedEventArgs e)
    {
        CreateWebTab();
    }

    private void Tabs_AddTabButtonClick(muxc.TabView sender, object args)
    {
        CreateWebTab();
    }

    public void CreateWebTab()
    {
        CreateTab("New tab", "\uEC6C", typeof(WebViewPage));
    }

    public void CreateTab(string header, string glyph, Type page)
    {
        Frame frame = new();
        muxc.TabViewItem newItem = new()
        {
            Header = header,
            IconSource = new muxc.FontIconSource { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets"), Glyph = glyph },
            Content = frame,
        };
        frame.Navigate(page);
        Tabs.TabItems.Add(newItem);
        Tabs.SelectedItem = newItem;
    }

    private void Tabs_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
    {
        muxc.TabViewItem selectedItem = args.Tab;
        var tabcontent = (Frame)selectedItem.Content;
        if (tabcontent.Content is WebViewPage) (tabcontent.Content as WebViewPage).WebViewControl.Close();
        sender.TabItems.Remove(args.Tab);
    }

    private void Tabs_TabItemsChanged(muxc.TabView sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
    {
        if (sender.TabItems.Count == 0)
        {
            CoreApplication.Exit();
        }
    }
}