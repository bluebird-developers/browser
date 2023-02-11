using Bluebird.Core;
using Windows.UI.Xaml.Controls;

namespace Bluebird.Pages.SettingPages;

public sealed partial class Privacy : Page
{
    public Privacy()
    {
        this.InitializeComponent();
        UpdateText();
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

    private void DisableJavaScriptToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        if (DisableJavaScriptToggle.IsOn)
        {
            SettingsHelper.SetSetting("DisableJavaScript", "true");
            trueCount++;
        }
        else
        {
            trueCount--;
            SettingsHelper.SetSetting("DisableJavaScript", "false");
        }
   
        UpdateText();
    }

    int trueCount = 0;
    public void UpdateText()
    {
        TextLevel.Text = trueCount switch
        {
            0 => "Default",
            1 => "Low",
            2 => "Medium",
            3 => "High",
            4 => "Extreme"
        };
    }

    private void DisableGenaralAutoFillToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        if (DisableGenaralAutoFillToggle.IsOn)
        {
            SettingsHelper.SetSetting("DisableGenAutoFill", "true");
            trueCount++;
        }
        else
        {
            SettingsHelper.SetSetting("DisableGenAutoFill", "false");
            trueCount--;
        }
        UpdateText();
    }

    private void DisablWebMessFillToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        if (DisableWebMessFillToggle.IsOn)
        {
            SettingsHelper.SetSetting("DisableWebMess", "true");
            trueCount++;
        }
        else
        {
            SettingsHelper.SetSetting("DisableWebMess", "false");
            trueCount--;
        }
        UpdateText();
    }

    private void PasswordWebMessFillToggle_Toggled(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        if (PasswordWebMessFillToggle.IsOn)
        {
            trueCount++;
            SettingsHelper.SetSetting("DisablePassSave", "true");
        }
        else
        {
            trueCount--;
            SettingsHelper.SetSetting("DisablePassSave", "false");
        }
        UpdateText();
    }

}
