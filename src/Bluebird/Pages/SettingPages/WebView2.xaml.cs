using Bluebird.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Bluebird.Pages.SettingPages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class WebView2 : Page
    {
        public WebView2()
        {
            this.InitializeComponent();
        }

        private void DisableTouch_Loaded(object sender, RoutedEventArgs e)
        {
            string selection2 = SettingsHelper.GetSetting("SwipeNav");
            if (selection2 == "true")
            {

            }
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
