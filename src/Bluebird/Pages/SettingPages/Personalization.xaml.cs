using Windows.UI.Xaml.Navigation;

namespace Bluebird.Pages.SettingPages;

public sealed partial class Personalization : Page
{
    public Personalization()
    {
        this.InitializeComponent();
        GetSettings();
    }

    private void GetSettings()
    {
        if (SettingsHelper.GetSetting("CompactTabs") == "true")
            CompactTabsToggle.IsOn = true;
        string UrlboxPos = SettingsHelper.GetSetting("UrlboxPos");
        if (UrlboxPos != null)
            UrlboxPosSelector.PlaceholderText = UrlboxPos;
        else
            UrlboxPosSelector.PlaceholderText = "Bottom";
        // Set event handlers
        CompactTabsToggle.Toggled += CompactTabsToggle_Toggled;
        UrlboxPosSelector.SelectionChanged += UrlboxPosSelector_SelectionChanged;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        CompactTabsToggle.Toggled -= CompactTabsToggle_Toggled;
        UrlboxPosSelector.SelectionChanged -= UrlboxPosSelector_SelectionChanged;
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

    private void UrlboxPosSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selection = e.AddedItems[0].ToString();
        if (selection == "Top")
            ViewModels.SettingsViewModel.SettingsVM.UrlboxPos = VerticalAlignment.Top;
        else
            ViewModels.SettingsViewModel.SettingsVM.UrlboxPos = VerticalAlignment.Bottom;
        SettingsHelper.SetSetting("UrlboxPos", selection);
    }
}
