using Microsoft.UI.Xaml.Controls;
using Bluebird.Shared;
using System;
using Windows.ApplicationModel.Core;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Bluebird.Pages;
using Bluebird.Core;
using System.Linq;
using Windows.UI.Xaml.Navigation;

namespace Bluebird;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        CustomTitleBar();
        Window.Current.VisibilityChanged += WindowVisibilityChangedEventHandler;
    }

    void WindowVisibilityChangedEventHandler(object sender, Windows.UI.Core.VisibilityChangedEventArgs e)
    {
        // Perform operations that should take place when the application becomes visible rather than
        // when it is prelaunched, such as building a what's new feed
        if (Tabs.TabItems.Count == 0) CreateHomeTab();
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

    private async void SidebarButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            switch ((sender as Button).Tag)
            {
                case "Back":
                    if (TabWebView != null) TabWebView.GoBack();
                    break;
                case "Refresh":
                    if (TabWebView != null) TabWebView.Reload();
                    break;
                case "Forward":
                    if (TabWebView != null) TabWebView.GoForward();
                    break;
                case "Search":
                    if (TabWebView != null)
                        UrlBox.Text = TabWebView.CoreWebView2.Source;
                    break;
                case "ReadingMode":
                    string jscript = await ReadingModeHelper.GetReadingModeJScriptAsync();
                    await TabWebView.CoreWebView2.ExecuteScriptAsync(jscript);
                    break;
                case "Translate":
                    string url = TabWebView.CoreWebView2.Source;
                    TabWebView.CoreWebView2.Navigate("https://translate.google.com/translate?hl&u=" + url);
                    break;
                case "Share":
                    if (TabWebView != null)
                        SystemHelper.ShowShareUIURL(TabWebView.CoreWebView2.DocumentTitle, TabWebView.CoreWebView2.Source);
                    break;
                case "AddFavoriteFlyout":
                    if (TabWebView != null)
                    {
                        AddFavoriteFlyout.ShowAt(AddFavoriteFlyoutBtn);
                        FavoriteTitle.Text = TabWebView.CoreWebView2.DocumentTitle;
                        FavoriteUrl.Text = TabWebView.CoreWebView2.Source;
                    }
                    break;
                case "AddFavorite":
                    FavoritesHelper.AddFavoritesItem(FavoriteTitle.Text, FavoriteUrl.Text);
                    AddFavoriteFlyout.Hide();
                    break;
                case "Favorites":
                    JsonItemsList = await Json.GetListFromJsonAsync("Favorites.json");
                    if (JsonItemsList != null) FavoritesListView.ItemsSource = JsonItemsList;
                    else
                    {
                        FavoritesListView.ItemsSource = null;
                    }
                    break;
                case "FavoritesExpanded":
                    OpenFavoriteFlyoutBtn.Flyout.Hide();
                    CreateTab("Favorites", Symbol.Favorite, typeof(FavoritesPage));
                    break;
            }
        }
        catch
        {
            await UI.ShowDialog("Error", "This action is not supported in this state");
        }
    }

    private async void MoreFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "Favorites":
                CreateTab("Favorites", Symbol.Favorite, typeof(FavoritesPage));
                break;
            case "History":
                launchurl = "edge://history";
                CreateWebTab();
                break;
            case "Fullscreen":
                var view = ApplicationView.GetForCurrentView();
                if (!view.IsFullScreenMode)
                {
                    WindowManager.EnterFullScreen(true);
                }
                else
                {
                    WindowManager.EnterFullScreen(false);
                }
                break;
            case "DevTools":
                if (TabWebView != null)
                {
                    TabWebView.CoreWebView2.OpenDevToolsWindow();
                }
                else
                {
                    await UI.ShowDialog("Error", "Only webpage source can be inspected");
                }
                break;
            case "ShowSource":
                if (TabWebView != null)
                {
                    launchurl = "view-source:" + TabWebView.Source.ToString();
                    CreateWebTab();
                }
                else
                {
                    await UI.ShowDialog("Error", "Only webpage source can be inspected");
                }
                break;
            case "Settings":
                CreateTab("Settings", Symbol.Setting, typeof(SettingsPage));
                break;
            case "About":
                break;
        }
    }

    #region UrlBox
    private void UrlBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            string input = UrlBox.Text;
            string inputtype = UrlHelper.GetInputType(input);
            if (input.Contains("Bluebird-int://"))
            {
                if (input == "Bluebird-int://newtab")
                {
                    TabContent.Navigate(typeof(NewTabPage));
                }
                if (input == "Bluebird-int://settings")
                {
                    TabContent.Navigate(typeof(SettingsPage));
                }
            }
            else if (inputtype == "url")
            {
                NavigateToUrl(input.Trim());
            }
            else if (inputtype == "urlNOProtocol")
            {
                NavigateToUrl("https://" + input.Trim());
            }
            else
            {
                string searchurl;
                if (SearchUrl == null) searchurl = "https://lite.qwant.com/?q=";
                else
                {
                    searchurl = SearchUrl;
                }
                string query = searchurl + input;
                NavigateToUrl(query);
            }
            SearchFlyout.Hide();
        }
    }

    private void UrlBox_GotFocus(object sender, RoutedEventArgs e)
    {
        UrlBox.SelectAll();
    }
    #endregion

    public void NavigateToUrl(string uri)
    {
        if (TabWebView != null)
        {
            TabWebView.CoreWebView2.Navigate(uri);
        }
        else
        {
            launchurl = uri;
            TabContent.Navigate(typeof(WebViewPage));
        }
    }

    public TabViewItem SelectedTab

    {
        get
        {
            TabViewItem selectedItem = (TabViewItem)Tabs.SelectedItem;
            if (selectedItem != null)
            {
                return selectedItem;
            }
            return null;
        }
    }

    Frame TabContent
    {
        get
        {
            TabViewItem selectedItem = (TabViewItem)Tabs.SelectedItem;
            if (selectedItem != null)
            {
                return (Frame)selectedItem.Content;
            }
            return null;
        }
    }

    WebView2 TabWebView
    {
        get
        {
            if (TabContent.Content is WebViewPage)
            {
                return (TabContent.Content as WebViewPage).WebViewControl;
            }
            return null;
        }
    }

    private void Tabs_Loaded(object sender, RoutedEventArgs e)
    {
        if (launchurl != null) CreateWebTab();
        else
        {
            CreateHomeTab();
        }
    }

    private void Tabs_AddTabButtonClick(TabView sender, object args)
    {
        CreateHomeTab();
    }

    private void Tabs_TabItemsChanged(TabView sender, Windows.Foundation.Collections.IVectorChangedEventArgs args)
    {
        // If there are no more tabs, close the browser
        if (sender.TabItems.Count == 0)
        {
            CoreApplication.Exit();
        }
    }

    public void CreateHomeTab()
    {
        CreateTab("New tab", Symbol.Document, typeof(NewTabPage));
    }

    public void CreateWebTab()
    {
        CreateTab("New tab", Symbol.Document, typeof(WebViewPage));
    }

    public void CreateTab(string header, Symbol symbol, Type page)
    {
        Frame frame = new();
        TabViewItem newItem = new()
        {
            Header = header,
            IconSource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = symbol },
            Content = frame,
        };
        frame.Navigate(page);
        Tabs.TabItems.Add(newItem);
        Tabs.SelectedItem = newItem;
    }

    private void Tabs_TabCloseRequested(TabView sender, TabViewTabCloseRequestedEventArgs args)
    {
        TabViewItem selectedItem = args.Tab;
        var tabcontent = (Frame)selectedItem.Content;
        if (tabcontent.Content is WebViewPage) (tabcontent.Content as WebViewPage).WebViewControl.Close();
        sender.TabItems.Remove(args.Tab);
    }

    private void FavoritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        // Get listview sender
        ListView listView = sender as ListView;
        if (listView.ItemsSource != null)
        {
            // Get selected item
            JsonItems item = (JsonItems)listView.SelectedItem;
            string url = item.Url;
            NavigateToUrl(url);
            OpenFavoriteFlyoutBtn.Flyout.Hide();
            listView.ItemsSource = null;
        }
    }

    private void Favorites_SearchBoxTextChanged(object sender, TextChangedEventArgs e)
    {
        TextBox textbox = sender as TextBox;
        // Get all ListView items with the submitted search query
        var SearchResults = from s in JsonItemsList where s.Title.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase) select s;
        // Set SearchResults as ItemSource for HistoryListView
        FavoritesListView.ItemsSource = SearchResults;
    }
}