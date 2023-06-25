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

    private void CompactTabsToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (CompactTabsToggle.IsOn)
            SettingsHelper.SetSetting("CompactTabs", "true");
        else
            SettingsHelper.SetSetting("CompactTabs", "true");
    }

    private void UrlboxPosSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selection = e.AddedItems[0].ToString();
        SettingsHelper.SetSetting("UrlboxPos", selection);
    }
}
