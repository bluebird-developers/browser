using System;
using Windows.ApplicationModel.DataTransfer;

namespace Bluebird.Shared
{
    public static class SystemHelper
    {

        public static string GetSystemArchitecture()
        {
            string architecture = Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE");
            return architecture;
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
    }
}
