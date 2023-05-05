using Bluebird.Pages;
using Windows.UI;
using Windows.UI.Xaml.Controls.Primitives;

namespace Bluebird;

public sealed partial class MainPage : Page
{
    public MainPage()
    {
        this.InitializeComponent();
        CustomTitleBar();
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

    private void ToolbarButton_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as Button).Tag)
        {
            case "Search":
                if (TabWebView != null)
                    UrlBox.Text = TabWebView.CoreWebView2.Source;
                break;
        }
    }

    private async void MoreFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "NewTab":
                CreateHomeTab();
                break;
            case "NewWindow":
                await Launcher.LaunchUriAsync(new Uri("bluebird:"));
                break;
            case "Downloads":
                launchurl = "edge://downloads";
                CreateWebTab();
                break;
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
                    WindowManager.EnterFullScreen(true);
                else
                    WindowManager.EnterFullScreen(false);
                break;
            case "Settings":
                CreateTab("Settings", Symbol.Setting, typeof(SettingsPage));
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
                    TabContent.Navigate(typeof(NewTabPage));
                if (input == "Bluebird-int://settings")
                    TabContent.Navigate(typeof(SettingsPage));
            }
            else if (inputtype == "url")
                NavigateToUrl(input.Trim());
            else if (inputtype == "urlNOProtocol")
                NavigateToUrl("https://" + input.Trim());
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
            TabWebView.CoreWebView2.Navigate(uri);
        else
        {
            launchurl = uri;
            TabContent.Navigate(typeof(WebViewPage));
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

    Frame TabContent
    {
        get
        {
            muxc.TabViewItem selectedItem = (muxc.TabViewItem)Tabs.SelectedItem;
            if (selectedItem != null)
                return (Frame)selectedItem.Content;
            return null;
        }
    }

    muxc.WebView2 TabWebView
    {
        get
        {
            if (TabContent.Content is WebViewPage)
                return (TabContent.Content as WebViewPage).WebViewControl;
            return null;
        }
    }

    private void Tabs_Loaded(object sender, RoutedEventArgs e)
    {
        if (launchurl != null)
            CreateWebTab();
        else
            CreateHomeTab();
    }

    private void Tabs_AddTabButtonClick(muxc.TabView sender, object args)
    {
        CreateHomeTab();
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
        muxc.TabViewItem newItem = new()
        {
            Header = header,
            IconSource = new muxc.SymbolIconSource() { Symbol = symbol },
            Content = frame,
        };
        frame.Navigate(page);
        Tabs.TabItems.Add(newItem);
        Tabs.SelectedItem = newItem;
    }

    private void Tabs_TabCloseRequested(muxc.TabView sender, muxc.TabViewTabCloseRequestedEventArgs args)
    {
        if (sender.TabItems.Count != 0)
        {
            muxc.TabViewItem selectedItem = args.Tab;
            var tabcontent = (Frame)selectedItem.Content;
            if (tabcontent.Content is WebViewPage) (tabcontent.Content as WebViewPage).WebViewControl.Close();
            sender.TabItems.Remove(args.Tab);
        }
        else
            CoreApplication.Exit();
    }
}