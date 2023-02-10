using Bluebird.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Bluebird.Pages.SettingPages
{
    public sealed partial class WebView2 : Page
    {
        public WebView2()
        {
            this.InitializeComponent();
            GetSettings();
        }

        private void GetSettings()
        {
            string SwipeNav = SettingsHelper.GetSetting("SwipeNav");
            if (SwipeNav == "true")
                DisableTouch.IsOn = true;
            // Set event handlers
            DisableTouch.Toggled += DisableTouch_Toggled;
        }

        private void DisableTouch_Toggled(object sender, RoutedEventArgs e)
        {
            if (DisableTouch.IsOn)
            {
                SettingsHelper.SetSetting("SwipeNav", "true");
            }
            else
            {
                SettingsHelper.SetSetting("SwipeNav", "false");
            }
        }
    }
}
