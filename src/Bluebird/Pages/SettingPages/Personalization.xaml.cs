using Windows.UI.Xaml.Navigation;

namespace Bluebird.Pages.SettingPages;

public sealed partial class Personalization : Page
{
    public Personalization()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        // Get settings and display them in the UI
        if (SettingsHelper.GetSetting("CompactTabs") == "true")
            CompactTabsToggle.IsOn = true;

        if (SettingsHelper.GetSetting("IsReadingModeEnabled") != "false")
            ReadingModeToggle.IsChecked = true;
        if (SettingsHelper.GetSetting("IsTranslateEnabled") != "false")
            TranslateToggle.IsChecked = true;
        if (SettingsHelper.GetSetting("IsQRCodeGenEnabled") != "false")
            QRCodeGenToggle.IsChecked = true;

        string UrlboxPos = SettingsHelper.GetSetting("UrlboxPos");
        if (UrlboxPos != null)
            UrlboxPosSelector.PlaceholderText = UrlboxPos;
        else
            UrlboxPosSelector.PlaceholderText = "Bottom";

        // Set event handlers
        CompactTabsToggle.Toggled += CompactTabsToggle_Toggled;

        ReadingModeToggle.Checked += ToolbarBtnCheckBox_Checked;
        TranslateToggle.Checked += ToolbarBtnCheckBox_Checked;
        QRCodeGenToggle.Checked += ToolbarBtnCheckBox_Checked;
        
        ReadingModeToggle.Unchecked += ToolbarBtnCheckBox_Unchecked;
        TranslateToggle.Unchecked += ToolbarBtnCheckBox_Unchecked;
        QRCodeGenToggle.Unchecked += ToolbarBtnCheckBox_Unchecked;

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

    private void ToolbarBtnCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        CheckBox chkbox = sender as CheckBox;
        SettingsHelper.SetSetting(chkbox.Tag.ToString(), "true");
        switch ((sender as CheckBox).Tag)
        {
            case "IsReadingModeEnabled":
                ViewModels.SettingsViewModel.SettingsVM.IsReadingModeEnabled = true;
                break;
            case "IsTranslateEnabled":
                ViewModels.SettingsViewModel.SettingsVM.IsTranslateEnabled = true;
                break;
            case "IsQRCodeGenEnabled":
                ViewModels.SettingsViewModel.SettingsVM.IsQRCodeGenEnabled = true;
                break;
        }
    }

    private void ToolbarBtnCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        CheckBox chkbox = sender as CheckBox;
        SettingsHelper.SetSetting(chkbox.Tag.ToString(), "false");
        switch ((sender as CheckBox).Tag)
        {
            case "IsReadingModeEnabled":
                ViewModels.SettingsViewModel.SettingsVM.IsReadingModeEnabled = false;
                break;
            case "IsTranslateEnabled":
                ViewModels.SettingsViewModel.SettingsVM.IsTranslateEnabled = false;
                break;
            case "IsQRCodeGenEnabled":
                ViewModels.SettingsViewModel.SettingsVM.IsQRCodeGenEnabled = false;
                break;
        }
    }
}
