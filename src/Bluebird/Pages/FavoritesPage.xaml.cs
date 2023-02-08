using Bluebird.Core;
using System;
using System.Linq;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Bluebird.Pages
{
    public sealed partial class FavoritesPage : Page
    {
        public FavoritesPage()
        {
            this.InitializeComponent();
            LoadFavorites();
        }

        private async void LoadFavorites()
        {
            JsonItemsList = await Json.GetListFromJsonAsync("Favorites.json");
            if (JsonItemsList != null)
                FavoritesListView.ItemsSource = JsonItemsList;
            else
                FavoritesListView.ItemsSource = null;
        }

        private void FavoritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Get listview sender
            ListView listView = sender as ListView;
            if (listView.ItemsSource != null)
            {
                // Get selected item
                JsonItems item = (JsonItems)listView.SelectedItem;
                launchurl = item.Url;
                MainPageContent.CreateWebTab();
            }
        }

        private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            TextBox textbox = sender as TextBox;
            // Get all ListView items with the submitted search query
            var SearchResults = from s in JsonItemsList where s.Title.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase) select s;
            // Set SearchResults as ItemSource for HistoryListView
            FavoritesListView.ItemsSource = SearchResults;
        }
    }
}
