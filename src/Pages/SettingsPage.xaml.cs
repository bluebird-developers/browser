using Windows.UI.Popups;

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
        if (SearchEngine != null)
            SearchEngineSelector.PlaceholderText = SearchEngine;
        else
            SearchEngineSelector.PlaceholderText = "Qwant";

        if (SettingsHelper.GetSetting("ForceDark") == "true")
            ForceDarkSwitch.IsOn = true;

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
        ForceDarkSwitch.Toggled += ForceDarkSwitch_Toggled;
        CompactTabsToggle.Toggled += CompactTabsToggle_Toggled;
        NewTabBgImageToggle.Toggled += NewTabBgImageToggle_Toggled;
        PasswordLockToggle.Toggled += PasswordLockToggle_Toggled;
    }

    private void SearchEngineSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selection = e.AddedItems[0].ToString();
        SearchEngineHelper.SetSearchEngine(selection);
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

    private void ForceDarkSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (ForceDarkSwitch.IsOn)
        {
            SettingsHelper.SetSetting("ForceDark", "true");
            SettingsViewModel.SettingsVM.IsForceDarkEnabled = true;
        }
        else
        {
            SettingsHelper.SetSetting("ForceDark", "false");
            SettingsViewModel.SettingsVM.IsForceDarkEnabled = false;
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
        (sender as TextBlock).Text = $"v{appversion} | {apparch}";
    }

    private async void OpenGitHubSettingsCard_Click(object sender, RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/bluebird-developers/browser"));
    }

    private async void OpenDevSanxDiscord_Click(object sender, RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://discord.com/invite/windows-apps-hub-714581497222398064"));
    }

    private async void OpenDonateLink_Click(object sender, RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://paypal.me/julianhasreiter"));
    }
}
