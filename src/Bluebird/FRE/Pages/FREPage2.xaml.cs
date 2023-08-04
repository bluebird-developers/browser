using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace Bluebird.FRE.Pages;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class FREPage2 : Page
{
    bool SearchEngineSelected = false;
    public FREPage2()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        SearchEngineSelector.Loaded += SearchEngineSelector_Loaded;
        SearchEngineSelector.SelectionChanged += SearchEngineSelector_SelectionChanged;
        FinishSetupBtn.Click += FinishSetupBtn_Click;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        SearchEngineSelector.Loaded -= SearchEngineSelector_Loaded;
        SearchEngineSelector.SelectionChanged -= SearchEngineSelector_SelectionChanged;
        FinishSetupBtn.Click -= FinishSetupBtn_Click;
    }

    private void SearchEngineSelector_Loaded(object sender, RoutedEventArgs e)
    {
        ComboBox comboBox = (sender as ComboBox);
        comboBox.ItemsSource = SearchEngineHelper.SearchEngines;
    }

    private void SearchEngineSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selection = e.AddedItems[0].ToString();
        SearchEngineHelper.SetSearchEngine(selection);
        SearchEngineSelected = true;
    }

    private async void FinishSetupBtn_Click(object sender, RoutedEventArgs e)
    {
        if (SearchEngineSelected)
        {
            SettingsHelper.SetSetting("OOBECompleted", "true");
            Frame.Navigate(typeof(MainPage), null, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }
        else
        {
            await UI.ShowDialog("Error", "Please select a search engine");
        }
    }
}
