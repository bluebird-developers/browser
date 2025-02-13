using CommunityToolkit.WinUI.Controls;
using System.Threading;

namespace Bluebird.Pages;

public sealed partial class SettingsPage : Page
{
    public SettingsPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        // Get settings and display them in the UI

        // General
        SearchEngineSelector.ItemsSource = SearchEngineHelper.SearchEngines;
        string SearchEngine = SettingsHelper.GetSetting("EngineFriendlyName");
        foreach (SearchEngine engine in SearchEngineHelper.SearchEngines)
        {
            if (engine.SearchUrl == SearchUrl)
            {
                SearchEngineSelector.SelectedItem = engine;
            }
        }

        AISearchEngineSelector.ItemsSource = SearchEngineHelper.AISearchEngines;
        string AISearchEngine = SettingsHelper.GetSetting("AIEngineFriendlyName");
        foreach (SearchEngine AIengine in SearchEngineHelper.AISearchEngines)
        {
            if (AIengine.SearchUrl == AISearchUrl)
            {
                AISearchEngineSelector.SelectedItem = AIengine;
            }
        }

        // Personalization
        if (SettingsHelper.GetSetting("CompactTabs") == "true")
            CompactTabsToggle.IsOn = true;

        if (SettingsHelper.GetSetting("IsNewTabWallpaperDisabled") == "true")
            NewTabBgImageToggle.IsOn = true;

        // Privacy
        if (SettingsHelper.GetSetting("PasswordLock") == "true")
            PasswordLockToggle.IsOn = true;

        // Set event handlers
        SearchEngineSelector.SelectionChanged += SearchEngineSelector_SelectionChanged;
        AISearchEngineSelector.SelectionChanged += AISearchEngineSelector_SelectionChanged;
        CompactTabsToggle.Toggled += CompactTabsToggle_Toggled;
        NewTabBgImageToggle.Toggled += NewTabBgImageToggle_Toggled;
        PasswordLockToggle.Toggled += PasswordLockToggle_Toggled;
    }

    private void SearchEngineSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SearchEngine engine = e.AddedItems[0] as SearchEngine;
        SearchEngineHelper.SetSearchEngine(engine, SearchEngineType.Classic);
    }

    private void AISearchEngineSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        SearchEngine engine = e.AddedItems[0] as SearchEngine;
        SearchEngineHelper.SetSearchEngine(engine, SearchEngineType.AI);
    }

    private async void ClearUserDataButton_Click(object sender, RoutedEventArgs e)
    {
        var result = await UI.ShowDialogWithAction($"Question", "Do you really want to clear all user data?", "Yes", "No");
        if (result == ContentDialogResult.Primary)
        {
            ClearUserDataProgressRing.IsActive = true;
            ClearUserDataBtn.IsEnabled = false;
            await WebView2ProfileDataHelper.ClearAllProfileDataAsync();
            ClearUserDataProgressRing.IsActive = false;
            ClearUserDataBtn.IsEnabled = true;
            ContentDialog dialog = new()
            {
                Title = "Info",
                Content = "User data was cleared",
                PrimaryButtonText = "Ok & restart app"
            };

            ContentDialogResult contentDialogResult = await dialog.ShowAsync();
            if (contentDialogResult == ContentDialogResult.Primary)
            {
                var appRestart = await CoreApplication.RequestRestartAsync(string.Empty);
                if (appRestart == AppRestartFailureReason.NotInForeground || appRestart == AppRestartFailureReason.RestartPending || appRestart == AppRestartFailureReason.Other)
                {
                    NotificationHelper.NotifyUser("Error", "Please restart Bluebird manually");
                }
            }
        }
    }

    private void CompactTabsToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (CompactTabsToggle.IsOn)
        {
            SettingsHelper.SetSetting("CompactTabs", "true");
            SettingsViewModel.SettingsVM.TabWidthMode = muxc.TabViewWidthMode.Compact;
        }
        else
        {
            SettingsHelper.SetSetting("CompactTabs", "false");
            SettingsViewModel.SettingsVM.TabWidthMode = muxc.TabViewWidthMode.Equal;
        }
    }

    private void NewTabBgImageToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (NewTabBgImageToggle.IsOn)
        {
            SettingsHelper.SetSetting("IsNewTabWallpaperDisabled", "true");
            SettingsViewModel.SettingsVM.IsNewTabWallpaperDisabled = true;
        }
        else
        {
            SettingsHelper.SetSetting("IsNewTabWallpaperDisabled", "false");
            SettingsViewModel.SettingsVM.IsNewTabWallpaperDisabled = false;
        }
    }

    private async void PasswordLockToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (PasswordLockToggle.IsOn)
        {
            PasswordBox pwBox = new();
            ContentDialog dialog = new()
            {
                Title = "Set password",
                Content = pwBox,
                PrimaryButtonText = "Save",
                SecondaryButtonText = "Cancel"
            };
            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string password = pwBox.Password;
                if (password != string.Empty)
                {
                    PasswordHelper.SaveCredential("User", password);
                    SettingsHelper.SetSetting("PasswordLock", "true");
                    return;
                }
                PasswordLockToggle.IsOn = false;
                SettingsHelper.SetSetting("PasswordLock", "false");
                return;
            }
            if (result == ContentDialogResult.Secondary || result == ContentDialogResult.None)
            {
                PasswordLockToggle.IsOn = false;
                SettingsHelper.SetSetting("PasswordLock", "false");
                return;
            }
        }
        if (!PasswordLockToggle.IsOn)
        {
            SettingsHelper.SetSetting("PasswordLock", "false");
        }
    }

    private void VersionTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        string appversion = AppVersion;
        string apparch = RuntimeInformation.ProcessArchitecture.ToString();
        (sender as TextBlock).Text = $"Version {appversion} | {apparch}";
    }

    private async void SettingsCardClickHandler(object sender, RoutedEventArgs e)
    {
        switch((sender as SettingsCard).Tag)
        {
            case "GitHub":
                await Launcher.LaunchUriAsync(new Uri("https://github.com/bluebird-developers/browser"));
                break;
            case "DevSanx":
                await Launcher.LaunchUriAsync(new Uri("https://discord.com/invite/windows-apps-hub-714581497222398064"));
                break;
            case "Donate":
                await Launcher.LaunchUriAsync(new Uri("https://paypal.me/julianhasreiter"));
                break;
            case "DebugSettings":
                MainPageContent.CreateTab("Debug settings", "\uF1AD", typeof(DebugSettings));
                break;
        }
    } 
}
