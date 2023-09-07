using Microsoft.Web.WebView2.Core;

namespace Bluebird.Pages;

public sealed partial class WebViewPage : Page
{
    public WebViewPage()
    {
        this.InitializeComponent();
        DataContext = ViewModels.SettingsViewModel.SettingsVM;
    }

    private void UrlBox_Loaded(object sender, RoutedEventArgs e)
    {
        UrlBox.Focus(FocusState.Programmatic);
    }

    private async void WebViewControl_Loaded(object sender, RoutedEventArgs e)
    {
        await (sender as muxc.WebView2).EnsureCoreWebView2Async();
    }

    private void WebViewControl_CoreWebView2Initialized(muxc.WebView2 sender, muxc.CoreWebView2InitializedEventArgs args)
    {
        // WebViewEvents
        sender.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
        sender.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        sender.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        sender.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
        sender.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
        sender.CoreWebView2.FaviconChanged += CoreWebView2_FaviconChanged;
        sender.CoreWebView2.ScriptDialogOpening += CoreWebView2_ScriptDialogOpening;
        sender.CoreWebView2.ContainsFullScreenElementChanged += CoreWebView2_ContainsFullScreenElementChanged;
        sender.CoreWebView2.LaunchingExternalUriScheme += CoreWebView2_LaunchingExternalUriScheme;
        // Apply WebView2 settings
        ApplyWebView2Settings(sender);
        if (launchurl != null)
        {
            sender.Source = new Uri(launchurl);
            launchurl = null;
        }
        else
            sender.Source = new Uri("https://bluebird-developers.github.io/ntp/");
    }

    private void ApplyWebView2Settings(muxc.WebView2 sender)
    {
        if (SettingsHelper.GetSetting("DisableJavaScript") is "true")
            sender.CoreWebView2.Settings.IsScriptEnabled = false;
        if (SettingsHelper.GetSetting("DisableGenAutoFill") is "true")
            sender.CoreWebView2.Settings.IsGeneralAutofillEnabled = false;
        if (SettingsHelper.GetSetting("DisablePassSave") is "true")
            sender.CoreWebView2.Settings.IsPasswordAutosaveEnabled = false;
        sender.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
        sender.CoreWebView2.Settings.UserAgent = $"{sender.CoreWebView2.Settings.UserAgent} Bluebird/{AppVersion.GetAppVersion()}";
    }

    private void CoreWebView2_NavigationStarting(CoreWebView2 sender, CoreWebView2NavigationStartingEventArgs args)
    {
        UrlBox.Text = args.Uri != "https://bluebird-developers.github.io/ntp/" ? args.Uri : UrlBox.Text;
        LoadingRing.IsActive = true;
    }

    private async void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        if (ViewModels.SettingsViewModel.SettingsVM.IsForceDarkEnabled)
        {
            string jscript = await Modules.ForceDark.ForceDarkHelper.GetForceDarkScriptAsync();
            await sender.ExecuteScriptAsync(jscript);
        }
        LoadingRing.IsActive = false;
    }

    private async void CoreWebView2_ScriptDialogOpening(CoreWebView2 sender, CoreWebView2ScriptDialogOpeningEventArgs args)
    {
        await UI.ShowDialog($"{sender.DocumentTitle} says", args.Message);
    }

    private void CoreWebView2_FaviconChanged(CoreWebView2 sender, object args)
    {
        MainPageContent.SelectedTab.IconSource = IconHelper.ConvFavURLToIconSource(sender.FaviconUri);
    }

    private void CoreWebView2_NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
    {
        launchurl = args.Uri;
        MainPageContent.CreateWebTab();
        args.Handled = true;
    }

    string SelectionText;
    string LinkUri;
    private void CoreWebView2_ContextMenuRequested(CoreWebView2 sender, CoreWebView2ContextMenuRequestedEventArgs args)
    {
        muxc.CommandBarFlyout flyout;
        if (args.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.SelectedText)
        {
            flyout = (muxc.CommandBarFlyout)Resources["TextContextMenu"];
            SelectionText = args.ContextMenuTarget.SelectionText;
        }

        else if (args.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.Image)
            flyout = null;

        else if (args.ContextMenuTarget.HasLinkUri)
        {
            flyout = (muxc.CommandBarFlyout)Resources["LinkContextMenu"];
            SelectionText = args.ContextMenuTarget.LinkText;
            LinkUri = args.ContextMenuTarget.LinkUri;
        }

        else if (args.ContextMenuTarget.IsEditable)
            flyout = null;

        else
            flyout = (muxc.CommandBarFlyout)Resources["PageContextMenu"];

        if (flyout != null)
        {
            FlyoutBase.SetAttachedFlyout(WebViewControl, flyout);
            var wv2flyout = FlyoutBase.GetAttachedFlyout(WebViewControl);
            var options = new FlyoutShowOptions()
            {
                Position = args.Location,
            };
            wv2flyout?.ShowAt(WebViewControl, options);
            args.Handled = true;
        }
    }

    private void CoreWebView2_DocumentTitleChanged(CoreWebView2 sender, object args)
    {
        MainPageContent.SelectedTab.Header = sender.DocumentTitle;
    }

    private void CoreWebView2_ContainsFullScreenElementChanged(CoreWebView2 sender, object args)
    {
        var view = ApplicationView.GetForCurrentView();
        if (!view.IsFullScreenMode)
            WindowManager.EnterFullScreen(true);
        else
            WindowManager.EnterFullScreen(false);
    }

    private async void CoreWebView2_LaunchingExternalUriScheme(CoreWebView2 sender, CoreWebView2LaunchingExternalUriSchemeEventArgs args)
    {
        var result = await UI.ShowDialogWithAction($"{sender.DocumentTitle} is trying to open an application", args.Uri, "Open", "Close");
        if (result == ContentDialogResult.Primary)
            await Launcher.LaunchUriAsync(new Uri(args.Uri));
    }

    private async void AppBarButton_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as AppBarButton).Tag)
        {
            // general context menu
            case "Back":
                if (WebViewControl.CanGoBack)
                    WebViewControl.GoBack();
                break;
            case "Refresh":
                WebViewControl.Reload();
                break;
            case "Forward":
                if (WebViewControl.CanGoForward)
                    WebViewControl.GoForward();
                break;
            case "Share":
                SystemHelper.ShowShareUIURL(WebViewControl.CoreWebView2.DocumentTitle, WebViewControl.CoreWebView2.Source);
                break;
            case "CopyPageLink":
                SystemHelper.WriteStringToClipboard(WebViewControl.CoreWebView2.Source);
                break;
            case "SelectAll":
                await WebViewControl.CoreWebView2.ExecuteScriptAsync("document.execCommand(\"selectAll\");");
                break;
            case "ShowAddFavoriteFlyout":
                FavoriteTitle.Text = WebViewControl.CoreWebView2.DocumentTitle;
                FavoriteUrl.Text = WebViewControl.CoreWebView2.Source;
                AddFavoriteFlyout.ShowAt(WebViewControl);
                break;
            // text context menu
            case "OpenLnkInNewTab":
                launchurl = LinkUri;
                MainPageContent.CreateWebTab();
                break;
            case "Copy":
                SystemHelper.WriteStringToClipboard(LinkUri);
                break;
            case "CopyText":
                SystemHelper.WriteStringToClipboard(SelectionText);
                break;
            // link context menu
            case "ShareLink":
                SystemHelper.ShowShareUIURL(SelectionText, LinkUri);
                break;
            case "Search":
                string searchurl;
                if (SearchUrl == null)
                    searchurl = "https://lite.qwant.com/?q=";
                else
                    searchurl = SearchUrl;
                string link = searchurl + SelectionText;
                launchurl = link;
                MainPageContent.CreateWebTab();
                break;
        }
        var flyout = FlyoutBase.GetAttachedFlyout(WebViewControl);
        flyout.Hide();
    }

    private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "ViewSource":
                launchurl = "view-source:" + WebViewControl.Source;
                MainPageContent.CreateWebTab();
                break;
            case "DevTools":
                WebViewControl.CoreWebView2.OpenDevToolsWindow();
                break;
            case "TaskManager":
                WebViewControl.CoreWebView2.OpenTaskManagerWindow();
                break;
        }
    }

    private void AddFavoriteButton_Click(object sender, RoutedEventArgs e)
    {
        FavoritesHelper.AddFavorite(FavoriteTitle.Text, FavoriteUrl.Text);
        AddFavoriteFlyout.Hide();
    }

    private void UrlBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            string input = UrlBox.Text;
            string inputtype = UrlHelper.GetInputType(input);
            if (inputtype == "urlNOProtocol")
                NavigateToUrl("https://" + input.Trim());
            else if (inputtype == "url")
                NavigateToUrl(input.Trim());
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
        }
    }

    private void UrlBox_GotFocus(object sender, RoutedEventArgs e)
    {
        UrlBox.SelectAll();
    }

    private void NavigateToUrl(string uri)
    {
        WebViewControl.CoreWebView2.Navigate(uri);
    }

    private async void ToolbarButton_Click(object sender, RoutedEventArgs e)
    {
        switch((sender as Button).Tag)
        {
            case "Back":
                WebViewControl.GoBack();
                break;
            case "Refresh":
                WebViewControl.Reload();
                break;
            case "ToggleUrlBox":
                UrlBoxWrapper.Visibility = UrlBoxWrapper.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
                UrlBox.Focus(FocusState.Keyboard);
                break;
            case "Forward":
                WebViewControl.GoForward();
                break;
            case "ReadingMode":
                string jscript = await Modules.Readability.ReadabilityHelper.GetReadabilityScriptAsync();
                await WebViewControl.CoreWebView2.ExecuteScriptAsync(jscript);
                break;
            case "Translate":
                string url = WebViewControl.CoreWebView2.Source;
                WebViewControl.CoreWebView2.Navigate("https://translate.google.com/translate?hl&u=" + url);
                break;
            case "AddFavoriteFlyout":
                FavoriteTitle.Text = WebViewControl.CoreWebView2.DocumentTitle;
                FavoriteUrl.Text = WebViewControl.CoreWebView2.Source;
                break;
            case "Downloads":
                WebViewControl.CoreWebView2.OpenDefaultDownloadDialog();
                break;
            case "GenQRCode":
                //Create raw qr code data
                QRCodeGenerator qrGenerator = new();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(WebViewControl.CoreWebView2.Source.ToString(), QRCodeGenerator.ECCLevel.M);

                //Create byte/raw bitmap qr code
                BitmapByteQRCode qrCodeBmp = new(qrCodeData);
                byte[] qrCodeImageBmp = qrCodeBmp.GetGraphic(20);
                using (InMemoryRandomAccessStream stream = new())
                {
                    using (DataWriter writer = new(stream.GetOutputStreamAt(0)))
                    {
                        writer.WriteBytes(qrCodeImageBmp);
                        await writer.StoreAsync();
                    }
                    var image = new BitmapImage();
                    await image.SetSourceAsync(stream);
                    // set image as image source for element
                    QRCodeImage.Source = image;
                }
                QRCodeFlyout.ShowAt(sender as Button);
                break;
        }
    }
}
