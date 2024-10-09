using Microsoft.Toolkit.Uwp.Notifications;

namespace Bluebird.Core
{
    internal class NotificationHelper
    {
        public static void NotifyUser(string header, string content)
        {
            var builder = new ToastContentBuilder()
                .AddText(header)
                .AddText(content);
            
            builder.Show(toast =>
            {
                 toast.ExpirationTime = DateTime.Now.AddMinutes(5);
            });
        }
    }
}
