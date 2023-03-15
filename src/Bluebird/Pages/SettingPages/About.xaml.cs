using Bluebird.Shared;
using Microsoft.Web.WebView2.Core;
using Windows.UI.Xaml.Controls;

namespace Bluebird.Pages.SettingPages;

public sealed partial class About : Page
{
    public About()
    {
        this.InitializeComponent();
        string appversion = AppVersion.GetAppVersion();
        string coreversion = CoreWebView2Environment.GetAvailableBrowserVersionString();
        VersionText.Text = $"Bluebird {appversion} (core: {coreversion})";
    }
}
