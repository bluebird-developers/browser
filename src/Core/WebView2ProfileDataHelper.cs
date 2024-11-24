namespace Bluebird.Core;

public static class WebView2ProfileDataHelper
{
    public static async Task ClearAllProfileDataAsync()
    {
        if (MainPageContent.mainWebView.CoreWebView2 != null) {
            CoreWebView2Profile profile = MainPageContent.mainWebView.CoreWebView2.Profile;
            await profile.ClearBrowsingDataAsync();
        }
        else
        {
            NotificationHelper.NotifyUser("Error", "An error occurred while trying to clear user profile data");
        }
        await FileHelper.DeleteLocalFile("Favorites.json");
        SettingsViewModel.SettingsVM.FavoritesList.Clear();
    }
}
