using Bluebird.Controls;
using Windows.UI.Xaml.Navigation;

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
        if (SettingsHelper.GetSetting("PasswordLock") == "true")
            PasswordLockToggle.IsOn = true;

        if (SettingsHelper.GetSetting("DisableJavaScript") == "true")
            DisableJavaScriptToggle.IsOn = true;

        if (SettingsHelper.GetSetting("DisableGenAutoFill") == "true")
            DisableGenaralAutoFillToggle.IsOn = true;

        if (SettingsHelper.GetSetting("DisablePassSave") == "true")
            PasswordWebMessFillToggle.IsOn = true;

        // Set event handlers
        PasswordLockToggle.Toggled += PasswordLockToggle_Toggled;
        DisableJavaScriptToggle.Toggled += DisableJavaScriptToggle_Toggled;
        DisableGenaralAutoFillToggle.Toggled += DisableGenaralAutoFillToggle_Toggled;
        PasswordWebMessFillToggle.Toggled += PasswordWebMessFillToggle_Toggled;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        PasswordLockToggle.Toggled -= PasswordLockToggle_Toggled;
        DisableJavaScriptToggle.Toggled -= DisableJavaScriptToggle_Toggled;
        DisableGenaralAutoFillToggle.Toggled -= DisableGenaralAutoFillToggle_Toggled;
        PasswordWebMessFillToggle.Toggled -= PasswordWebMessFillToggle_Toggled;
    }

    private async void PasswordLockToggle_Toggled(object sender, RoutedEventArgs e)
    {
        if (PasswordLockToggle.IsOn)
        {
            PasswordContentDialog dialog = new();
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
