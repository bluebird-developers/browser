namespace Bluebird;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        Window.Current.SetTitleBar(CustomDragRegion);
        DataContext = SettingsViewModel.SettingsVM;
    }

    private void BrowserMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "Downloads":
                CreateTab("Downloads", "\uE896", typeof(WebViewPage), "edge://downloads");
                break;
            case "Favorites":
                FavoritesFlyout.ShowAt(BrowserMenuBtn);
                FavoritesListView.SelectedItem = null;
                break;
            case "History":
                CreateTab("History", "\uE81C", typeof(WebViewPage), "edge://history");
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

    private void Tabs_Loaded(object sender, RoutedEventArgs e)
    {
        string startupurl = AppStartupHelper.GetStartupUrl();
        if (startupurl != null)
        {
            CreateWebTab(startupurl);
            return;
        }
        CreateTab("New tab", "\uEC6C", typeof(NewTabPage));
    }

    private void NewTabButton_Click(muxc.SplitButton sender, muxc.SplitButtonClickEventArgs args)
    {
        CreateTab("New tab", "\uEC6C", typeof(NewTabPage));
    }

    private void NewTabFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "NewTab":
                CreateTab("New tab", "\uEC6C", typeof(NewTabPage));
                break;
            case "NewSplitTab":
                CreateTab("Split tab", "\uF57C", typeof(SplitTabPage));
                break;
        }
    }

    public void CreateWebTab(string url = null)
    {
        if (url != null)
        {
            CreateTab("New tab", "\uEC6C", typeof(WebViewPage), url);
            return;
        }

        CreateTab("New tab", "\uEC6C", typeof(WebViewPage));
    }

    public void CreateTab(string header, string glyph, Type page, string url = null)
    {
        Frame frame = new();
        muxc.TabViewItem newItem = new()
        {
            Header = header,
            IconSource = new muxc.FontIconSource { FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets"), Glyph = glyph },
            Content = frame,
        };

        if (url != null)
        {
            WebTabCreationParams parameters = new()
            {
                Url = url,
                myTab = newItem
            };
            frame.Navigate(page, parameters);
        }
        else
        {
            XAMLTabCreationParams parameters = new()
            {
                myTab = newItem
            };
            frame.Navigate(page, parameters);
        }
        
        Tabs.TabItems.Add(newItem);
        Tabs.SelectedItem = newItem;
    }

    private void Tabs_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
    {
        muxc.TabViewItem selectedItem = args.Tab;
        var tabcontent = (Frame)selectedItem.Content;
        if (tabcontent.Content is WebViewPage) (tabcontent.Content as WebViewPage).WebViewControl.Close();
        if (tabcontent.Content is SplitTabPage) (tabcontent.Content as SplitTabPage).CloseWebViews();
        sender.TabItems.Remove(selectedItem);
        // Workaround for memory leak in TabView
        // microsoft-ui-xaml issue #3597
        // https://github.com/microsoft/microsoft-ui-xaml/issues/3597
        GC.Collect();
    }

    private void Tabs_TabItemsChanged(muxc.TabView sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
    {
        if (sender.TabItems.Count == 0)
        {
            Application.Current.Exit();
        }
    }

    #region Favorites flyout
    private void FavoritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListView listView = sender as ListView;
        if (listView.SelectedItem != null)
        {
            FavoriteItem item = (FavoriteItem)listView.SelectedItem;
            CreateWebTab(item.Url);
            FavoritesFlyout.Hide();
        }
    }

    FavoriteItem selectedItem;
    private void FavoritesListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
        selectedItem = ((FrameworkElement)e.OriginalSource).DataContext as FavoriteItem;
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