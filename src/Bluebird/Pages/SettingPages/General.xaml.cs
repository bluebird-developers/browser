using Windows.UI.Xaml.Navigation;

namespace Bluebird.Pages.SettingPages;

public sealed partial class General : Page
{
    public General()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        // Get settings and display them in the UI
        SearchEngineSelector.ItemsSource = SearchEngineHelper.SearchEngines;
        string SearchEngine = SettingsHelper.GetSetting("EngineFriendlyName");
        if (SearchEngine != null)
            SearchEngineSelector.PlaceholderText = SearchEngine;
        else
            SearchEngineSelector.PlaceholderText = "Qwant Lite";

        if (SettingsHelper.GetSetting("ForceDark") == "true")
            ForceDarkSwitch.IsOn = true;
        // Set event handlers
        SearchEngineSelector.SelectionChanged += SearchEngineSelector_SelectionChanged;
        ForceDarkSwitch.Toggled += ForceDarkSwitch_Toggled;
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        SearchEngineSelector.SelectionChanged -= SearchEngineSelector_SelectionChanged;
        ForceDarkSwitch.Toggled -= ForceDarkSwitch_Toggled;
    }

    private void SearchEngineSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        string selection = e.AddedItems[0].ToString();
        SearchEngineHelper.SetSearchEngine(selection);
    }

    private void ForceDarkSwitch_Toggled(object sender, RoutedEventArgs e)
    {
        if (ForceDarkSwitch.IsOn)
        {
            SettingsHelper.SetSetting("ForceDark", "true");
            ViewModels.SettingsViewModel.SettingsVM.IsForceDarkEnabled = true;
        }
        else
        {
            SettingsHelper.SetSetting("ForceDark", "false");
            ViewModels.SettingsViewModel.SettingsVM.IsForceDarkEnabled = false;
        }
    }
}
