using System.Runtime.InteropServices;
using Windows.UI.Xaml.Navigation;

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

        // Privacy
        if (SettingsHelper.GetSetting("PasswordLock") == "true")
            PasswordLockToggle.IsOn = true;

        // Set event handlers
        SearchEngineSelector.SelectionChanged += SearchEngineSelector_SelectionChanged;
        ForceDarkSwitch.Toggled += ForceDarkSwitch_Toggled;
        CompactTabsToggle.Toggled += CompactTabsToggle_Toggled;
        PasswordLockToggle.Toggled += PasswordLockToggle_Toggled;
    }

    private void SearchEngineSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selection = e.AddedItems[0].ToString();
        SearchEngineHelper.SetSearchEngine(selection);
    }

    private void ForceDarkSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (ForceDarkSwitch.IsOn)
        {
            SettingsHelper.SetSetting("ForceDark", "true");
            ViewModels.SettingsViewModel.SettingsVM.IsForceDarkEnabled = true;
        }
        else
        {
            SettingsHelper.SetSetting("ForceDark", "false");
            ViewModels.SettingsViewModel.SettingsVM.IsForceDarkEnabled = false;
        }
    }

    private void CompactTabsToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (CompactTabsToggle.IsOn)
        {
            SettingsHelper.SetSetting("CompactTabs", "true");
            ViewModels.SettingsViewModel.SettingsVM.TabWidthMode = muxc.TabViewWidthMode.Compact;
        }
        else
        {
            SettingsHelper.SetSetting("CompactTabs", "false");
            ViewModels.SettingsViewModel.SettingsVM.TabWidthMode = muxc.TabViewWidthMode.Equal;
        }
    }

    private async void PasswordLockToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (PasswordLockToggle.IsOn)
        {
            Dialogs.PasswordContentDialog dialog = new();
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
                SettingsHelper.SetSetting("PasswordLock", "true");
            else
            {
                PasswordLockToggle.IsOn = false;
                SettingsHelper.SetSetting("PasswordLock", "false");
                PasswordHelper.RemoveCredential("User");
            }
        }
        else
        {
            SettingsHelper.SetSetting("PasswordLock", "false");
            PasswordHelper.RemoveCredential("User");
        }
    }

    private void VersionTextBlock_Loaded(object sender, RoutedEventArgs e)
    {
        string appversion = AppVersion.GetAppVersion();
        string apparch = RuntimeInformation.ProcessArchitecture.ToString();
        (sender as TextBlock).Text = $"v{appversion} | {apparch}";
    }

    private async void OpenGitHubSettingsCard_Click(object sender, RoutedEventArgs e)
    {
        await Launcher.LaunchUriAsync(new Uri("https://github.com/bluebird-developers/browser"));
    }
}
