namespace Bluebird.Pages;

public sealed partial class FavoritesPage : Page
{
    public static List<JsonItems> FavoritesList;
    public FavoritesPage()
    {
        this.InitializeComponent();
        LoadFavorites();
    }

    private async void LoadFavorites()
    {
        FavoritesList = await Json.GetListFromJsonAsync("Favorites.json");
        if (FavoritesList != null)
            FavoritesListView.ItemsSource = FavoritesList;
        else
            FavoritesListView.ItemsSource = null;
    }

    private void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        TextBox textbox = sender as TextBox;
        // Get all ListView items with the submitted search query
        var SearchResults = from s in FavoritesList where s.Title.Contains(textbox.Text, StringComparison.OrdinalIgnoreCase) select s;
        // Set SearchResults as ItemSource for HistoryListView
        FavoritesListView.ItemsSource = SearchResults;
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        LoadFavorites();
    }

    private async void ClearAllButton_Click(object sender, RoutedEventArgs e)
    {
        ContentDialogResult result = await UI.ShowDialogWithAction("Delete all favorites?", "Do you want to clear all favorites?", "Clear", "Cancel");

        if (result == ContentDialogResult.Primary)
        {
            await FileHelper.DeleteLocalFile("Favorites.json");
            FavoritesListView.ItemsSource = null;
        }
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

    string ctmtext;
    string ctmurl;
    private void FavoritesListView_RightTapped(object sender, RightTappedRoutedEventArgs e)
    {
        ListView listView = sender as ListView;
        var options = new FlyoutShowOptions()
        {
            Position = e.GetPosition(listView),
        };
        FavoritesContextMenu.ShowAt(listView, options);
        var item = ((FrameworkElement)e.OriginalSource).DataContext as JsonItems;
        ctmtext = item.Title;
        ctmurl = item.Url;
    }

    private async void FavContextItem_Click(object sender, RoutedEventArgs e)
    {
        switch ((sender as AppBarButton).Tag)
        {
            case "OpenLnkInNewWindow":
                await Launcher.LaunchUriAsync(new Uri($"{ctmurl}"));
                break;
            case "Copy":
                SystemHelper.WriteStringToClipboard(ctmurl);
                break;
            case "CopyText":
                SystemHelper.WriteStringToClipboard(ctmtext);
                break;
            case "ShareLink":
                SystemHelper.ShowShareUIURL(ctmtext, ctmurl);
                break;
        }
        FavoritesContextMenu.Hide();
    }
}
