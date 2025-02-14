namespace Bluebird.Core;

public static class NotificationHelper
{
    private static XmlDocument xmlDoc;
    public static void NotifyUser(string header, string content)
    {
        string xmlString = $"<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastGeneric\"><text>{header}</text><text>{content}</text></binding></visual></toast>";
        xmlDoc ??= new XmlDocument();
        xmlDoc.LoadXml(xmlString);
        ToastNotification notification = new(xmlDoc)
        {
            ExpirationTime = DateTime.Now.AddMinutes(5)
        };
        ToastNotificationManager.CreateToastNotifier().Show(notification);
    }
}
