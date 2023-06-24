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
        // Set event handlers
        CompactTabsToggle.Toggled += CompactTabsToggle_Toggled;
    }

    private void CompactTabsToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (CompactTabsToggle.IsOn)
            SettingsHelper.SetSetting("CompactTabs", "true");
        else
            SettingsHelper.SetSetting("CompactTabs", "true");
    }
}
