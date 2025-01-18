using CommunityToolkit.WinUI.Controls;

namespace Bluebird.Pages;

public sealed partial class DebugSettings : Page
{
    public DebugSettings()
    {
        this.InitializeComponent();
    }

    private async void AppLangInput_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            MultiLanguageHelper.OverrideLanguage((sender as TextBox).Text);
            await UI.ShowDialog("Success", "Your selection has been saved. Please restart the app to apply them");
        }
    }

    private async void SettingsCard_Click(object sender, RoutedEventArgs e)
    {
        switch((sender as SettingsCard).Tag)
        {
            case "PrintAppInfo":
                string dotnetver = Environment.Version.ToString();
                string appver = AppVersionHelper.GetAppVersion();
                string apparch = RuntimeInformation.ProcessArchitecture.ToString();
                string sysarch = RuntimeInformation.OSArchitecture.ToString();
                string sysversion = Environment.OSVersion.VersionString;

                string wv2version = CoreWebView2Environment.GetAvailableBrowserVersionString();

                string debugCombinedString = $"Bluebird Version {appver}\n.NET Version: {dotnetver}\nAppArch: {apparch}\nSys: {sysversion}\nSysArch: {sysarch}\nWebView2Runtime: {wv2version}";
                
                await UI.ShowDialog("App info", debugCombinedString);
                break;
        }
    }
}
