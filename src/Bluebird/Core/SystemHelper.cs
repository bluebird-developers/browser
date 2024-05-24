using Windows.ApplicationModel.DataTransfer;

namespace Bluebird.Core;

public static class SystemHelper
{
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
