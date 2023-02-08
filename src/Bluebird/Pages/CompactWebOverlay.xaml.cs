using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bluebird.Pages
{
    public sealed partial class CompactWebOverlay : Page
    {
        public CompactWebOverlay()
        {
            this.InitializeComponent();
            CompactWebView.EnsureCoreWebView2Async();
        }

        private void CompactWebView_CoreWebView2Initialized(Microsoft.UI.Xaml.Controls.WebView2 sender, Microsoft.UI.Xaml.Controls.CoreWebView2InitializedEventArgs args)
        {
            sender.CoreWebView2.NavigationStarting += CoreWebView2_NavigationStarting;
            sender.CoreWebView2.NavigationCompleted += CoreWebView2_NavigationCompleted;
            sender.Source = new Uri(launchurl);
            launchurl = null;
        }

        private void CoreWebView2_NavigationStarting(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationStartingEventArgs args)
        {
            LoadingRing.IsActive = true;
        }

        private void CoreWebView2_NavigationCompleted(Microsoft.Web.WebView2.Core.CoreWebView2 sender, Microsoft.Web.WebView2.Core.CoreWebView2NavigationCompletedEventArgs args)
        {
            LoadingRing.IsActive = false;
        }

        private void ToolbarButton_Click(object sender, RoutedEventArgs e)
        {
            switch ((sender as Button).Tag)
            {
                case "Back":
                    if (CompactWebView.CanGoBack)
                        CompactWebView.GoBack();
                    break;
                case "Refresh":
                    CompactWebView.Reload();
                    break;
                case "Forward":
                    if (CompactWebView.CanGoForward)
                        CompactWebView.GoForward();
                    break;
                case "Close":
                    // TODO: Ask the user if they want to release the site into a tab
                    // or close it entirely
                    CompactWebView.Close();
                    MainPageContent.CompactOverlayFrame.Visibility = Visibility.Collapsed;
                    break;
            }
        }
    }
}
