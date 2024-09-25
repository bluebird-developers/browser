﻿using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace Bluebird.Pages
{
    public sealed partial class NewTabPage : Page
    {
        public NewTabPage()
        {
            this.InitializeComponent();
            //UserFavoritesListView.ItemsSource = SettingsViewModel.SettingsVM.FavoritesList;
            if (!ViewModels.SettingsViewModel.SettingsVM.IsNewTabWallpaperDisabled)
            {
                string wallpaperPath = NewTabHelper.GetRandomWallpaper();
                rootGrid.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(wallpaperPath)), Stretch = Stretch.UniformToFill };
            }
        }

        private void UrlBox_Loaded(object sender, RoutedEventArgs e)
        {
            (sender as TextBox).Focus(FocusState.Programmatic);
        }

        private void UrlBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                string input = (sender as TextBox).Text;
                string inputtype = UrlHelper.GetInputType(input);
                if (inputtype == "urlNOProtocol")
                    NavigateToUrl("https://" + input.Trim());
                else if (inputtype == "url")
                    NavigateToUrl(input.Trim());
                else
                {
                    string query = SearchUrl + input;
                    NavigateToUrl(query);
                }
            }
        }

        private void NavigateToUrl(string query)
        {
            WebTabCreationParams parameters = new() { Url = query };
            Frame.Navigate(typeof(WebViewPage), parameters, new DrillInNavigationTransitionInfo());
        }

        /*private void UserFavoritesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView listView = sender as ListView;
            if (listView.SelectedItem != null)
            {
                FavoriteItems item = (FavoriteItems)listView.SelectedItem;
                NavigateToUrl(item.Url)
            }
        }*/
    }
}
