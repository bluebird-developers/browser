using System.Runtime.InteropServices;

namespace Bluebird.Controls;

public sealed partial class AboutContentDialog : ContentDialog
{
    public AboutContentDialog() => this.InitializeComponent();

    private void BluebirdVersionDisplay_Loaded(object sender, RoutedEventArgs e)
    {
        string appversion = AppVersion.GetAppVersion();
        string apparch = RuntimeInformation.ProcessArchitecture.ToString();
        BluebirdVersionDisplay.Text = $"Bluebird, version {appversion} ({apparch})";
    }

    private void WebView2VersionDisplay_Loaded(object sender, RoutedEventArgs e)
    {
        string wv2version = CoreWebView2Environment.GetAvailableBrowserVersionString();
        WebView2VersionDisplay.Text = $"WebView2 Runtime, version {wv2version}";
    }
}
