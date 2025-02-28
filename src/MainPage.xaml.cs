namespace Bluebird;

public sealed partial class MainPage : Page
{
    public muxc.WebView2 MainWebView { get; set; }

    public MainPage()
    {
        this.InitializeComponent();
        Window.Current.SetTitleBar(CustomDragRegion);
        var coreWindow = CoreWindow.GetForCurrentThread();
        coreWindow.KeyDown += CoreWindow_KeyDown;
        InitWebView();
    }

    private void CoreWindow_KeyDown(CoreWindow sender, KeyEventArgs args)
    {
        var ctrlState = CoreWindow.GetForCurrentThread().GetKeyState(VirtualKey.Control);
        var isCtrlDown = (ctrlState & CoreVirtualKeyStates.Down) == CoreVirtualKeyStates.Down;

        if (isCtrlDown && args.VirtualKey == VirtualKey.T)
        {
            CreateTab("New tab", "\uEC6C", typeof(NewTabPage));
            args.Handled = true;
        }
    }

    private async void InitWebView()
    {
        // preloads WebView2 for faster initial navigation
        try
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () =>
            {
                MainWebView = new muxc.WebView2();
                await MainWebView.EnsureCoreWebView2Async();
            });
        }
        catch
        {
            var result = await UI.ShowDialogWithAction("Error", "WebView2 Runtime is not installed which is required to display webpages", "Download WebView2 Runtime", "Close App");
            if (result == ContentDialogResult.Primary)
                await Launcher.LaunchUriAsync(new Uri("https://go.microsoft.com/fwlink/p/?LinkId=2124703"));
            else
                CoreApplication.Exit();
        }
    }

    private void BrowserMenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "RecentlyClosedFlyout":
                RecentlyClosedTabsListView.SelectedItem = null;
                RecentlyClosedTabsFlyout.ShowAt(BrowserMenuBtn);
                break;
            case "Downloads":
                CreateTab("Downloads", "\uE896", typeof(WebViewPage), "edge://downloads");
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
            frame.Navigate(page, parameters, new DrillInNavigationTransitionInfo());
        }
        else
        {
            XAMLTabCreationParams parameters = new()
            {
                myTab = newItem
            };
            frame.Navigate(page, parameters, new DrillInNavigationTransitionInfo());
        }
        
        Tabs.TabItems.Add(newItem);
        Tabs.SelectedItem = newItem;
    }

    private void Tabs_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
    {
        muxc.TabViewItem tab = args.Tab;
        var tabcontent = (Frame)tab.Content;
        if (tabcontent.Content is WebViewPage)
        {
            WebViewPage content = tabcontent.Content as WebViewPage;
            string title = content.WebViewControl.CoreWebView2.DocumentTitle;
            string url = content.WebViewControl.CoreWebView2.Source;
            content.WebViewControl.Close();
            FavoriteItem newItem = new()
            {
                Title = title,
                Url = url
            };
            SettingsViewModel.SettingsVM.RecentlyClosedTabsList.Insert(0, newItem);
        }
        if (tabcontent.Content is SplitTabPage)
        {
            (tabcontent.Content as SplitTabPage).CloseWebViews();
        }
        Tabs.TabItems.Remove(tab);
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
    private void FavoritesFlyoutButton_Click(object sender, RoutedEventArgs e)
    {
        FavoritesFlyout.ShowAt(BrowserMenuBtn);
        FavoritesListView.SelectedItem = null;
    }

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

    private void RecentlyClosedTabsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListView listView = sender as ListView;
        if (listView.SelectedItem != null)
        {
            FavoriteItem item = (FavoriteItem)listView.SelectedItem;
            CreateWebTab(item.Url);
            RecentlyClosedTabsFlyout.Hide();
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