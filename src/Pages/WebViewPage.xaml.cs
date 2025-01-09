namespace Bluebird.Pages;

public sealed partial class WebViewPage : Page
{
    private string launchurl;
    private muxc.TabViewItem myTab;
    private bool IsSplitTab;

    public WebViewPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter != null)
        {
            var parameters = (WebTabCreationParams)e.Parameter;

            launchurl = parameters.Url;
            myTab = parameters.myTab;
            IsSplitTab = parameters.IsSplitTab;
        }
    }

    private async void WebViewControl_Loaded(object sender, RoutedEventArgs e)
    {
        // check if CoreWebView2 has been initalized
        // if not, ensure it has been initialized
        if (WebViewControl.CoreWebView2 == null)
        {
            try
            {
                await (sender as muxc.WebView2).EnsureCoreWebView2Async();
            }
            catch (Exception ex)
            {
                WebViewControl?.Close();
                Frame.Navigate(typeof(WebViewErrorPage), new WebView2CreationError(ex.StackTrace), new DrillInNavigationTransitionInfo());
            }
        }
    }

    private async void WebViewControl_CoreWebView2Initialized(muxc.WebView2 sender, muxc.CoreWebView2InitializedEventArgs args)
    {
        // WebViewEvents
        sender.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
        sender.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
        sender.CoreWebView2.NewWindowRequested += CoreWebView2_NewWindowRequested;
        sender.CoreWebView2.SourceChanged += CoreWebView2_SourceChanged;
        sender.CoreWebView2.ContextMenuRequested += CoreWebView2_ContextMenuRequested;
        if (!IsSplitTab)
        {
            sender.CoreWebView2.DocumentTitleChanged += CoreWebView2_DocumentTitleChanged;
            sender.CoreWebView2.FaviconChanged += CoreWebView2_FaviconChanged;
        }
        sender.CoreWebView2.ContainsFullScreenElementChanged += CoreWebView2_ContainsFullScreenElementChanged;
        sender.CoreWebView2.LaunchingExternalUriScheme += CoreWebView2_LaunchingExternalUriScheme;
        sender.CoreWebView2.IsDocumentPlayingAudioChanged += CoreWebView2_IsDocumentPlayingAudioChanged;
        sender.CoreWebView2.IsMutedChanged += CoreWebView2_IsMutedChanged;
        sender.WebMessageReceived += CoreWebView2_WebMessageReceived;
        // Apply WebView2 settings
        ApplyWebView2Settings(sender);
        string mainscript = "document.addEventListener('keydown', function(event) { if (event.ctrlKey && event.key === 't') { event.preventDefault(); window.chrome.webview.postMessage('ControlT'); } });";
        await sender.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(mainscript);
        if (launchurl != null)
        {
            NavigateToUrl(launchurl);
            launchurl = null;
        }
        else
        {
            sender.NavigateToString(ModernBlankPage.MinifiedModernBlackPageHTML);
        }
    }

    private void ApplyWebView2Settings(muxc.WebView2 sender)
    {
        sender.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = false;
        sender.CoreWebView2.Settings.UserAgent = $"{sender.CoreWebView2.Settings.UserAgent} Bluebird/{AppVersion}";
    }

    private void CoreWebView2_NavigationStarting(CoreWebView2 sender, CoreWebView2NavigationStartingEventArgs args)
    {
        LoadingBar.IsIndeterminate = true;
        LoadingBar.Visibility = Visibility.Visible;
    }

    private void CoreWebView2_NavigationCompleted(CoreWebView2 sender, CoreWebView2NavigationCompletedEventArgs args)
    {
        LoadingBar.Visibility = Visibility.Collapsed;
        LoadingBar.IsIndeterminate = false;
    }

    private void CoreWebView2_SourceChanged(CoreWebView2 sender, CoreWebView2SourceChangedEventArgs args)
    {
        UrlBox.Text = sender.Source;
    }

    private void CoreWebView2_FaviconChanged(CoreWebView2 sender, object args)
    {
        myTab.IconSource = IconHelper.ConvFavURLToIconSource(sender.FaviconUri);
    }

    private void CoreWebView2_NewWindowRequested(CoreWebView2 sender, CoreWebView2NewWindowRequestedEventArgs args)
    {
        MainPageContent.CreateWebTab(args.Uri);
        args.Handled = true;
    }

    string SelectionText;
    string LinkUri;
    private void CoreWebView2_ContextMenuRequested(CoreWebView2 sender, CoreWebView2ContextMenuRequestedEventArgs args)
    {
        if (sender.Source == "about:blank")
        {
            args.Handled = true;
            return;
        }

        muxc.CommandBarFlyout flyout;
        if (args.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.SelectedText)
        {
            flyout = (muxc.CommandBarFlyout)Resources["TextContextMenu"];
            SelectionText = args.ContextMenuTarget.SelectionText;
        }

        else if (args.ContextMenuTarget.Kind == CoreWebView2ContextMenuTargetKind.Image)
            return;

        else if (args.ContextMenuTarget.HasLinkUri)
        {
            flyout = (muxc.CommandBarFlyout)Resources["LinkContextMenu"];
            SelectionText = args.ContextMenuTarget.LinkText;
            LinkUri = args.ContextMenuTarget.LinkUri;
        }

        else if (args.ContextMenuTarget.IsEditable)
            return;

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
        myTab.Header = sender.DocumentTitle;
    }

    private void CoreWebView2_ContainsFullScreenElementChanged(CoreWebView2 sender, object args)
    {
        var view = ApplicationView.GetForCurrentView();
        if (!view.IsFullScreenMode)
        {
            WindowManager.EnterFullScreen(true);
            UrlBoxWrapper.Visibility = Visibility.Collapsed;
            Sidebar.Visibility = Visibility.Collapsed;
        }
        else
        {
            WindowManager.EnterFullScreen(false);
            UrlBoxWrapper.Visibility = Visibility.Visible;
            Sidebar.Visibility = Visibility.Visible;
        }
    }

    private async void CoreWebView2_LaunchingExternalUriScheme(CoreWebView2 sender, CoreWebView2LaunchingExternalUriSchemeEventArgs args)
    {
        var result = await UI.ShowDialogWithAction($"{sender.DocumentTitle} is trying to open an application", args.Uri, "Open", "Close");
        if (result == ContentDialogResult.Primary)
            await Launcher.LaunchUriAsync(new Uri(args.Uri));
    }

    private void CoreWebView2_IsDocumentPlayingAudioChanged(CoreWebView2 sender, object args)
    {
        if (sender.IsDocumentPlayingAudio)
        {
            MuteBtn.Visibility = Visibility.Visible;
            return;
        }

        if (!sender.IsDocumentPlayingAudio)
        {
            MuteBtn.Visibility = Visibility.Collapsed;
            return;
        }
    }

    private void CoreWebView2_IsMutedChanged(CoreWebView2 sender, object args)
    {
        if (sender.IsMuted)
        {
            MuteBtn.Content = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\uE74F" };
            return;
        }
        if (!sender.IsMuted)
        {
            MuteBtn.Content = new FontIcon { FontFamily = new FontFamily("Segoe MDL2 Assets"), Glyph = "\uE767" };
        }
    }

    private void CoreWebView2_WebMessageReceived(muxc.WebView2 sender, CoreWebView2WebMessageReceivedEventArgs args)
    {
        // this input has been treated as VERY unsecure input
        // DO NOT add anything which could be slighly insecure
        if (args.TryGetWebMessageAsString() == "ControlT")
        {
            MainPageContent.CreateTab("New tab", "\uEC6C", typeof(NewTabPage));
        }
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
            case "Translate":
                string url = WebViewControl.CoreWebView2.Source;
                WebViewControl.CoreWebView2.Navigate("https://translate.google.com/translate?hl&u=" + url);
                break;
            // text context menu
            case "OpenLnkInNewTab":
                MainPageContent.CreateWebTab(LinkUri);
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
                string link = SearchUrl + SelectionText;
                MainPageContent.CreateWebTab(link);
                break;
        }
        var flyout = FlyoutBase.GetAttachedFlyout(WebViewControl);
        flyout.Hide();
    }

    private async void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as MenuFlyoutItem).Tag)
        {
            case "ExportAsPdf":
                using (IRandomAccessStream fileStream = await WebViewControl.CoreWebView2.PrintToPdfStreamAsync(null))
                {
                    using (var reader = new DataReader(fileStream.GetInputStreamAt(0)))
                    {
                        await reader.LoadAsync((uint)fileStream.Size);
                        var buffer = new byte[(int)fileStream.Size];
                        reader.ReadBytes(buffer);
                        await FileHelper.SaveBytesAsFileAsync($"{WebViewControl.CoreWebView2.DocumentTitle}.pdf", buffer, "Portable Document File", ".pdf");
                    }
                }
                break;
            case "ExportAsTxt":
                string script = "document.body.innerText";
                string textContent = await WebViewControl.CoreWebView2.ExecuteScriptAsync(script);
                await FileHelper.SaveStringAsFileAsync($"{WebViewControl.CoreWebView2.DocumentTitle}", textContent, "Text file", ".txt");
                break;
            case "ViewSource":
                MainPageContent.CreateWebTab("view-source:" + WebViewControl.Source);
                break;
            /*case "DevTools":
                WebViewControl.CoreWebView2.OpenDevToolsWindow();
                break;*/
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

    #region "UrlBox event handlers"
    private void UrlBox_GotFocus(object sender, RoutedEventArgs e) => (sender as TextBox).SelectAll();

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
                string query = SearchUrl + input;
                NavigateToUrl(query);
            }
        }
    }

    private void AISearchButton_Click(object sender, RoutedEventArgs e)
    {
        string query = UrlBox.Text;
        if (!string.IsNullOrEmpty(query))
        {
            NavigateToUrl(AISearchUrl + query);
        }
    }
    #endregion

    private void NavigateToUrl(string uri)
    {
        WebViewControl.CoreWebView2.Navigate(uri);
    }

    bool IsInReadingMode = false;
    byte[] QrCode;
    private async void SidebarButton_Click(object sender, RoutedEventArgs e)
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
                if (IsInReadingMode)
                {
                    (sender as Button).Background = null;
                    WebViewControl.Reload();
                    UrlBoxWrapper.Visibility = Visibility.Visible;
                    IsInReadingMode = false;
                    break;
                }
                if (!IsInReadingMode)
                {
                    string jscript = await Modules.Readability.ReadabilityHelper.GetReadabilityScriptAsync();
                    await WebViewControl.CoreWebView2.ExecuteScriptAsync(jscript);
                    Brush accentBrush = Application.Current.Resources["SystemControlHighlightAccentBrush"] as SolidColorBrush;
                    (sender as Button).Background = accentBrush;
                    UrlBoxWrapper.Visibility = Visibility.Collapsed;
                    IsInReadingMode = true;
                    break;
                }
                break;
            case "AddFavoriteFlyout":
                FavoriteTitle.Text = WebViewControl.CoreWebView2.DocumentTitle;
                FavoriteUrl.Text = WebViewControl.CoreWebView2.Source;
                break;
            case "Downloads":
                WebViewControl.CoreWebView2.OpenDefaultDownloadDialog();
                break;
            case "GenQRCode":
                QrCode = await QRCodeHelper.GenerateQRCodeFromUrlAsync(WebViewControl.CoreWebView2.Source);
                if (QrCode == null)
                {
                    await UI.ShowDialog("Error", "An error occured while trying to create a qr code for this website");
                    break;
                }
                BitmapImage QrCodeImage = await QRCodeHelper.ConvertBitmapBytesToImage(QrCode);
                QRCodeImage.Source = QrCodeImage;
                QRCodeFlyout.ShowAt(sender as Button);
                break;
            case "Mute":
                WebViewControl.CoreWebView2.IsMuted = !WebViewControl.CoreWebView2.IsMuted;
                break;
        }
    }

    private async void QRCodeButton_Click(object sender, RoutedEventArgs e)
    {
        await FileHelper.SaveBytesAsFileAsync("QRCode", QrCode, "Bitmap", ".bmp");
    }
}
