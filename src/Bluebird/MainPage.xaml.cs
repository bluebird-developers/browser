using Bluebird.Pages;

namespace Bluebird;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        Window.Current.SetTitleBar(CustomDragRegion);
        DataContext = ViewModels.SettingsViewModel.SettingsVM;
    }

    private void BrowserMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "Downloads":
                WebTabCreationParams downloadParameter = new()
                {
                    Url = "edge://downloads"
                };
                CreateTab("Downloads", "\uE896", typeof(WebViewPage), downloadParameter);
                break;
            case "Favorites":
                FavoritesFlyout.ShowAt(BrowserMenuBtn);
                FavoritesListView.SelectedItem = null;
                break;
            case "History":
                WebTabCreationParams historyParameter = new()
                {
                    Url = "edge://history"
                };
                CreateTab("History", "\uE81C", typeof(WebViewPage), historyParameter);
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
        string startupurl = AppStartupHelper.GetStartupUrl();
        if (startupurl != null)
        {
            CreateWebTab(startupurl);
            return;
        }
        CreateWebTab();
    }

    private void NewTabButton_Click(muxc.SplitButton sender, muxc.SplitButtonClickEventArgs args)
    {
        CreateWebTab();
    }

    private void NewTabFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "NewTab":
                CreateWebTab();
                break;
            case "NewSplitTab":
                CreateTab("New split tab", "\uF57C", typeof(SplitTabPage));
                break;
        }
    }

    public void CreateWebTab(string url = null)
    {
        if (url != null)
        {
            WebTabCreationParams parameters = new()
            {
                Url = url
            };
            CreateTab("New tab", "\uEC6C", typeof(WebViewPage), parameters);
            return;
        }

        CreateTab("New tab", "\uEC6C", typeof(WebViewPage));
    }

    public void CreateTab(string header, string glyph, Type page, WebTabCreationParams parameters = null)
    {
        Frame frame = new();
        muxc.TabViewItem newItem = new()
        {
            Header = header,
            IconSource = new muxc.FontIconSource { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets"), Glyph = glyph },
            Content = frame,
        };
        if (parameters != null)
            frame.Navigate(page, parameters);
        else
            frame.Navigate(page);
        
        Tabs.TabItems.Add(newItem);
        Tabs.SelectedItem = newItem;
    }

    private void Tabs_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
    {
        muxc.TabViewItem selectedItem = args.Tab;
        var tabcontent = (Frame)selectedItem.Content;
        if (tabcontent.Content is WebViewPage) (tabcontent.Content as WebViewPage).WebViewControl.Close();
        if (tabcontent.Content is SplitTabPage) (tabcontent.Content as SplitTabPage).CloseWebViews();
        sender.TabItems.Remove(args.Tab);
        // Workaround for memory leak in TabView
        // microsoft-ui-xaml issue #3597
        // https://github.com/microsoft/microsoft-ui-xaml/issues/3597
        GC.Collect();
    }

    private void Tabs_TabItemsChanged(muxc.TabView sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
    {
        if (sender.TabItems.Count == 0)
        {
            CoreApplication.Exit();
        }
    }

    #region Favorites flyout
    private void FavoritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListView listView = sender as ListView;
        if (listView.SelectedItem != null)
        {
            FavoriteItems item = (FavoriteItems)listView.SelectedItem;
            CreateWebTab(item.Url);
            FavoritesFlyout.Hide();
        }
    }

    FavoriteItems selectedItem;
    private void FavoritesListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
        selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as FavoriteItems;
    }

    private void FavContextItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as AppBarButton).Tag)
        {
            case "Copy":
                SystemHelper.WriteStringToClipboard(selectedItem.Url);
                break;
            case "ShareLink":
                SystemHelper.ShowShareUIURL(selectedItem.Title, selectedItem.Url);
                break;
            case "Delete":
                FavoritesListView.SelectedItem = null;
                FavoritesHelper.RemoveFavorite(selectedItem);
                break;
            case "CopyText":
                SystemHelper.WriteStringToClipboard(selectedItem.Title);
                break;
        }
        FavoritesContextMenu.Hide();
    }
    #endregion
}