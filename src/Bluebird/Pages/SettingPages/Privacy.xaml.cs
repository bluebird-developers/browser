namespace Bluebird.Pages.SettingPages;

public sealed partial class Privacy : Page
{
    public Privacy()
    {
        this.InitializeComponent();
        GetSettings();
    }

    private void GetSettings()
    {
        string DisableJS = SettingsHelper.GetSetting("DisableJavaScript");
        if (DisableJS == "true")
            DisableJavaScriptToggle.IsOn = true;

        string DisableAutoFill = SettingsHelper.GetSetting("DisableGenAutoFill");
        if (DisableAutoFill == "true")
            DisableGenaralAutoFillToggle.IsOn = true;

        string DisableWebMess = SettingsHelper.GetSetting("DisableWebMess");
        if (DisableWebMess == "true")
            DisableWebMessFillToggle.IsOn = true;

        string DisablePassSave = SettingsHelper.GetSetting("DisablePassSave");
        if (DisablePassSave == "true")
            PasswordWebMessFillToggle.IsOn = true;

        // Set event handlers
        DisableJavaScriptToggle.Toggled += DisableJavaScriptToggle_Toggled;
        DisableGenaralAutoFillToggle.Toggled += DisableGenaralAutoFillToggle_Toggled;
        DisableWebMessFillToggle.Toggled += DisableGenaralAutoFillToggle_Toggled;
        PasswordWebMessFillToggle.Toggled += PasswordWebMessFillToggle_Toggled;
    }

    private void DisableJavaScriptToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (DisableJavaScriptToggle.IsOn)
        {
            SettingsHelper.SetSetting("DisableJavaScript", "true");
        }
        else
        {
            SettingsHelper.SetSetting("DisableJavaScript", "false");
        }
    }
    private void DisableGenaralAutoFillToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (DisableGenaralAutoFillToggle.IsOn)
        {
            SettingsHelper.SetSetting("DisableGenAutoFill", "true");
        }
        else
        {
            SettingsHelper.SetSetting("DisableGenAutoFill", "false");
        }
    }

    private void DisablWebMessFillToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (DisableWebMessFillToggle.IsOn)
        {
            SettingsHelper.SetSetting("DisableWebMess", "true");
        }
        else
        {
            SettingsHelper.SetSetting("DisableWebMess", "false");
        }
    }

    private void PasswordWebMessFillToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (PasswordWebMessFillToggle.IsOn)
        {
            SettingsHelper.SetSetting("DisablePassSave", "true");
        }
        else
        {
            SettingsHelper.SetSetting("DisablePassSave", "false");
        }
    }

}
