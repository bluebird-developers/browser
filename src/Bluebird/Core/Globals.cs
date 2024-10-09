namespace Bluebird.Core;

public class Globals
{
    // Access public elements/methods from MainPage
    public static MainPage MainPageContent
    {
        get { return (Window.Current.Content as Frame)?.Content as MainPage; }
    }
    // Global variables
    public static StorageFolder localFolder = ApplicationData.Current.LocalFolder;
    public static string SearchUrl { get; set; }
    public static string StartupUrl { get; set; }
}
