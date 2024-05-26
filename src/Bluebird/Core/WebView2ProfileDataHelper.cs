namespace Bluebird.Core
{
    public static class WebView2ProfileDataHelper
    {
        public static async void ClearAllProfileData()
        {
            muxc.WebView2 HeadlessWebView2 = new();
            await HeadlessWebView2.EnsureCoreWebView2Async();
            if (HeadlessWebView2.CoreWebView2 != null)
            {
                CoreWebView2Profile profile = HeadlessWebView2.CoreWebView2.Profile;
                await profile.ClearBrowsingDataAsync();
            }
            HeadlessWebView2.Close();
            StorageFile file = await localFolder.GetFileAsync("Favorites.json");
            await file.DeleteAsync();
        }
    }
}
