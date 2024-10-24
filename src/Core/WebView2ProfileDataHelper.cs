﻿namespace Bluebird.Core;

public static class WebView2ProfileDataHelper
{
    public static async Task ClearAllProfileDataAsync()
    {
        muxc.WebView2 HeadlessWebView2 = new();
        await HeadlessWebView2.EnsureCoreWebView2Async();
        if (HeadlessWebView2.CoreWebView2 != null)
        {
            CoreWebView2Profile profile = HeadlessWebView2.CoreWebView2.Profile;
            await profile.ClearBrowsingDataAsync();
        }
        HeadlessWebView2.Close();
        await FileHelper.DeleteLocalFile("Favorites.json");
        SettingsViewModel.SettingsVM.FavoritesList.Clear();
    }
}
