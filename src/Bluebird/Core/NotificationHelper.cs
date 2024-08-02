using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bluebird.Core
{
    internal class NotificationHelper
    {
        public static async void NotifyUser(string header, string content)
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
