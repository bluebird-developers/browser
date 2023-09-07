using Bluebird.ViewModels;

namespace Bluebird.Core;

public class Globals
{
    public class FavoriteItems
    {
        public string Title { get; set; }
        public string Url { get; set; }
    }

    // Access public elements/methods from MainPage
    public static MainPage MainPageContent
    {
        get { return (Window.Current.Content as Frame)?.Content as MainPage; }
    }
    // Global variables
    public static StorageFolder localFolder = ApplicationData.Current.LocalFolder;

    public static string HomepageUrl { get; set; }
    public static string SearchUrl { get; set; }

    // Varible which defines which url should be launched when a new tab is created
    public static string launchurl { get; set; }
}
