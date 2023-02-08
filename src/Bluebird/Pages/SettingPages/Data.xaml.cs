using Bluebird.Core;
using Bluebird.Shared;
using Windows.UI.Xaml.Controls;

namespace Bluebird.Pages.SettingPages;

public sealed partial class Data : Page
{
    public Data()
    {
        this.InitializeComponent();
    }

    private async void ClearBrowserData_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
    {
        ContentDialogResult result = await UI.ShowDialogWithAction("Clear data", "Do you want to clear all your browsing data including: Favorites and History", "Clear", "Cancel");

        if (result == ContentDialogResult.Primary)
            await FileHelper.DeleteLocalFile("Favorites.json");
    }
}
