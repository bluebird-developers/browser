﻿using Bluebird.Pages;
using Windows.ApplicationModel.Activation;
using Windows.UI;
using Windows.UI.Xaml.Navigation;

namespace Bluebird;

/// <summary>
/// Stellt das anwendungsspezifische Verhalten bereit, um die Standardanwendungsklasse zu ergänzen.
/// </summary>
sealed partial class App : Application
{
    /// <summary>
    /// Initialisiert das Singletonanwendungsobjekt. Dies ist die erste Zeile von erstelltem Code
    /// und daher das logische Äquivalent von main() bzw. WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();
        this.Suspending += OnSuspending;
        LoadSettings();
        FavoritesHelper.LoadFavoritesOnStartup();
    }

    private void LoadSettings()
    {
        SearchUrl = SettingsHelper.GetSetting("SearchUrl") ?? "https://www.qwant.com/?q=";

        if (SettingsHelper.GetSetting("ForceDark") == "true")
            ViewModels.SettingsViewModel.SettingsVM.IsForceDarkEnabled = true;
        if (SettingsHelper.GetSetting("CompactTabs") == "true")
            ViewModels.SettingsViewModel.SettingsVM.TabWidthMode = muxc.TabViewWidthMode.Compact;
        else
            ViewModels.SettingsViewModel.SettingsVM.TabWidthMode = muxc.TabViewWidthMode.Equal;
    }

    protected override void OnActivated(IActivatedEventArgs args)
    {
        if (args.Kind == ActivationKind.Protocol)
        {
            ProtocolActivatedEventArgs eventArgs = args as ProtocolActivatedEventArgs;
            StartupUrl = eventArgs.Uri.ToString();
            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                SetupTitleBar();
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new();

                rootFrame.NavigationFailed += OnNavigationFailed;

                Window.Current.Content = rootFrame;

                string PasswordLock = SettingsHelper.GetSetting("PasswordLock");
                if (PasswordLock == "true")
                    rootFrame.Navigate(typeof(LoginPage));
                else
                    rootFrame.Navigate(typeof(MainPage));
                // Ensure the current window is active
                Window.Current.Activate();
            }
            else
            {
                MainPageContent.CreateWebTab(StartupUrl);
            }
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn die Anwendung durch den Endbenutzer normal gestartet wird. Weitere Einstiegspunkte
    /// werden z. B. verwendet, wenn die Anwendung gestartet wird, um eine bestimmte Datei zu öffnen.
    /// </summary>
    /// <param name="e">Details über Startanforderung und -prozess.</param>
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
                    rootFrame.Navigate(typeof(LoginPage));
                else
                    rootFrame.Navigate(typeof(MainPage), e.Arguments);

            }
            // Ensure the current window is active
            Window.Current.Activate();
        }
    }

    /// <summary>
    /// Wird aufgerufen, wenn die Navigation auf eine bestimmte Seite fehlschlägt
    /// </summary>
    /// <param name="sender">Der Rahmen, bei dem die Navigation fehlgeschlagen ist</param>
    /// <param name="e">Details über den Navigationsfehler</param>
    void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
    {
        throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
    }

    /// <summary>
    /// Wird aufgerufen, wenn die Ausführung der Anwendung angehalten wird.  Der Anwendungszustand wird gespeichert,
    /// ohne zu wissen, ob die Anwendung beendet oder fortgesetzt wird und die Speicherinhalte dabei
    /// unbeschädigt bleiben.
    /// </summary>
    /// <param name="sender">Die Quelle der Anhalteanforderung.</param>
    /// <param name="e">Details zur Anhalteanforderung.</param>
    private void OnSuspending(object sender, SuspendingEventArgs e)
    {
        var deferral = e.SuspendingOperation.GetDeferral();
        //TODO: Anwendungszustand speichern und alle Hintergrundaktivitäten beenden
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
    }
}
