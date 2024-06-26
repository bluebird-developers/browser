﻿using Bluebird.Pages;

namespace Bluebird;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        Window.Current.SetTitleBar(CustomDragRegion);
        DataContext = ViewModels.SettingsViewModel.SettingsVM;
    }

    private void MoreFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        BrowserMenuFlyout.Hide();
        switch ((sender as AppBarButton).Tag)
        {
            case "NewTab":
                CreateWebTab();
                break;
            case "NewSplitTab":
                CreateTab("New split tab", "\uF57C", typeof(SplitTabPage));
                break;
            case "NewWindow":
                break;
            case "Downloads":
                launchurl = "edge://downloads";
                CreateTab("Downloads", "\uE896", typeof(WebViewPage));
                break;
            case "Favorites":
                FavoritesFlyout.ShowAt(BrowserMenuBtn);
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
        // Get listview sender
        ListView listView = sender as ListView;
        if (listView.SelectedItem != null)
        {
            // Get selected item
            FavoriteItems item = (FavoriteItems)listView.SelectedItem;
            launchurl = item.Url;
            CreateWebTab();
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