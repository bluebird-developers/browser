using System;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;

namespace Bluebird.Core;

public static class SystemHelper
{
    public static async void RestartApp()
    {
        AppRestartFailureReason result = await CoreApplication.RequestRestartAsync("restart");

        if (result == AppRestartFailureReason.NotInForeground
            || result == AppRestartFailureReason.Other)
        {
            await UI.ShowDialog("Error", "Couldn´t restart Bluebird, please close the app manually");
        }
    }
    
    public static void ShowShareUIURL(string title, string url)
    {
        var dt = DataTransferManager.GetForCurrentView();
        dt.DataRequested += (sender, args) =>
        {
            DataRequest request = args.Request;
            request.Data.SetWebLink(new Uri(url));
            request.Data.Properties.Title = title;
        };
        DataTransferManager.ShowShareUI();
    }

    public static void WriteStringToClipboard(string text)
    {
        DataPackage dataPackage = new()
        {
            RequestedOperation = DataPackageOperation.Copy
        };
        dataPackage.SetText(text);
        Clipboard.SetContent(dataPackage);
    }
}
