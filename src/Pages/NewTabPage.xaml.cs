namespace Bluebird.Pages;

public sealed partial class NewTabPage : Page
{
    private muxc.TabViewItem myTab;

    public NewTabPage()
    {
        this.InitializeComponent();
        //UserFavoritesListView.ItemsSource = SettingsViewModel.SettingsVM.FavoritesList;
        if (!SettingsViewModel.SettingsVM.IsNewTabWallpaperDisabled)
        {
            string wallpaperPath = NewTabHelper.GetRandomWallpaper();
            rootGrid.Background = new ImageBrush { ImageSource = new BitmapImage(new Uri(wallpaperPath)), Stretch = Stretch.UniformToFill };
        }
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (e.Parameter != null)
        {
            var parameters = (XAMLTabCreationParams)e.Parameter;

            myTab = parameters.myTab;
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
            if (!string.IsNullOrEmpty(input))
            {
                ProcessQueryAndGo(input);
            }
        }
    }

    private void NavigateToUrl(string query)
    {
        WebTabCreationParams parameters = new() { Url = query, myTab = myTab };
        Frame.Navigate(typeof(WebViewPage), parameters, new DrillInNavigationTransitionInfo());
    }

    private void AISearchButton_Click(object sender, RoutedEventArgs e)
    {
        string query = UrlBox.Text;
        if (!string.IsNullOrEmpty(query))
        {
            NavigateToUrl(AISearchUrl + query);
        }
    }

    private void ProcessQueryAndGo(string input)
    {
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

    /*private void UserFavoritesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        ListView listView = sender as ListView;
        if (listView.SelectedItem != null)
        {
            FavoriteItem item = (FavoriteItem)listView.SelectedItem;
            NavigateToUrl(item.Url);
        }
    }*/
}
