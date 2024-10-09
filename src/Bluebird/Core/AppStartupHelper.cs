namespace Bluebird.Core;

public static class AppStartupHelper
{
    public static string GetStartupUrl()
    {
        if (StartupUrl == null)
            return null;

        string url = StartupUrl;
        StartupUrl = null;
        return url;
    }
}
