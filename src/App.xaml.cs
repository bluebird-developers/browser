namespace Bluebird;

sealed partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
        this.Suspending += OnSuspending;
        this.UnhandledException += App_UnhandledException;
        LoadSettings();
        FavoritesHelper.LoadFavoritesOnStartup();
    }

    private void App_UnhandledException(object sender, Windows.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        NotificationHelper.NotifyUser("An error occured", "An unhandled exception occured, the app my be in an unstable state.\nPlease restart Bluebird as soon as possible.");
        e.Handled = true;
    }

    private void LoadSettings()
    {
        AISearchUrl = SettingsHelper.GetSetting("AISearchUrl") ?? "https://www.perplexity.ai/search?q=";
        SearchUrl = SettingsHelper.GetSetting("SearchUrl") ?? "https://www.qwant.com/?q=";

        if (SettingsHelper.GetSetting("ForceDark") == "true")
            SettingsViewModel.SettingsVM.IsForceDarkEnabled = true;
        if (SettingsHelper.GetSetting("CompactTabs") == "true")
            SettingsViewModel.SettingsVM.TabWidthMode = muxc.TabViewWidthMode.Compact;
        else
            SettingsViewModel.SettingsVM.TabWidthMode = muxc.TabViewWidthMode.Equal;
        if (SettingsHelper.GetSetting("IsNewTabWallpaperDisabled") == "true")
            SettingsViewModel.SettingsVM.IsNewTabWallpaperDisabled = true;
        else
            SettingsViewModel.SettingsVM.IsNewTabWallpaperDisabled = false;
    }

    protected override void OnActivated(IActivatedEventArgs args)
    {
        if (args.Kind == ActivationKind.Protocol)
        {
            ProtocolActivatedEventArgs ProtocolEventArgs = args as ProtocolActivatedEventArgs;
            StartupUrl = ProtocolEventArgs.Uri.ToString();
            UnifiedArgumentStartup();
        }

        if (args.Kind == ActivationKind.CommandLineLaunch)
        {
            CommandLineActivatedEventArgs CommandEventArgs = args as CommandLineActivatedEventArgs;
            CommandLineActivationOperation operation = CommandEventArgs.Operation;
            string input = operation.Arguments;

            // input validation to prevent various issues
            // keep in sync with WebViewPage.xaml.cs and NewTabPage.xaml.cs
            string inputtype = UrlHelper.GetInputType(input);
            if (inputtype == "urlNOProtocol")
                StartupUrl = "https://" + input.Trim();
            else if (inputtype == "url")
                StartupUrl = input.Trim();
            else
            {
                string query = SearchUrl + input;
                StartupUrl = query;
            }
            UnifiedArgumentStartup();
        }
    }

    private void UnifiedArgumentStartup()
    {
        Frame rootFrame = Window.Current.Content as Frame;

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (rootFrame == null)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new();

            rootFrame.NavigationFailed += OnNavigationFailed;

            Window.Current.Content = rootFrame;

            SetupTitleBar();
            string PasswordLock = SettingsHelper.GetSetting("PasswordLock");
            if (PasswordLock == "true")
                rootFrame.Navigate(typeof(LoginPage), null, new DrillInNavigationTransitionInfo());
            else
                rootFrame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
            // Ensure the current window is active
            Window.Current.Activate();
        }
        else
        {
            MainPageContent.CreateWebTab(StartupUrl);
        }
    }

    protected override void OnLaunched(LaunchActivatedEventArgs e)
    {
        Frame rootFrame = Window.Current.Content as Frame;

        // Do not repeat app initialization when the Window already has content,
        // just ensure that the window is active
        if (rootFrame == null)
        {
            // Create a Frame to act as the navigation context and navigate to the first page
            rootFrame = new();

            rootFrame.NavigationFailed += OnNavigationFailed;

            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }

        if (e.PrelaunchActivated == false)
        {
            // On Windows 10 version 1607 or later, this code signals that this app wants to participate in prelaunch
            // Since Bluebird does not run on <1809, no check is required
            TryEnablePrelaunch();

            // TODO: This is not a prelaunch activation. Perform operations which
            // assume that the user explicitly launched the app such as updating
            // the online presence of the user on a social network, updating a
            // what's new feed, etc.

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                SetupTitleBar();
                string PasswordLock = SettingsHelper.GetSetting("PasswordLock");
                if (PasswordLock == "true")
                    rootFrame.Navigate(typeof(LoginPage), null, new DrillInNavigationTransitionInfo());
                else
                    rootFrame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());

            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
    }

    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        var deferral = e.SuspendingOperation.GetDeferral();
        deferral.Complete();
    }

    /// <summary>
    /// This method should be called only when the caller
    /// determines that we're running on a system that
    /// supports CoreApplication.EnablePrelaunch.
    /// </summary>
    private void TryEnablePrelaunch()
    {
        CoreApplication.EnablePrelaunch(true);
    }

    private void SetupTitleBar()
    {
        // Hide default title bar.
        var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        coreTitleBar.ExtendViewIntoTitleBar = true;
        var titleBar = ApplicationView.GetForCurrentView().TitleBar;
        // Set colors
        titleBar.ButtonBackgroundColor = Colors.Transparent;
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
    }

#if DEBUG
    private void OverrideLanguage(string langCode)
    {
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = langCode;
    }

    private string GetPreferredLanguage()
    {
        return Windows.Globalization.ApplicationLanguages.Languages[0];
    }
#endif
}
