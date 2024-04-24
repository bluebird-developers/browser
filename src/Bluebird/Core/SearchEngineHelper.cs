namespace Bluebird.Core;

public class SearchEngineHelper
{
    public static void SetSearchEngine(string selection)
    {
        if (selection == "Ask") SetEngine("Ask", "https://www.ask.com/web?q=");
        if (selection == "Baidu") SetEngine("Baidu", "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd=");
        if (selection == "Bing") SetEngine("Bing", "https://www.bing.com?q=");
        if (selection == "Brave Search") SetEngine("Brave Search", "https://search.brave.com/search?q=");
        if (selection == "DuckDuckGo") SetEngine("DuckDuckGo", "https://www.duckduckgo.com?q=");
        if (selection == "Ecosia") SetEngine("Ecosia", "https://www.ecosia.org/search?q=");
        if (selection == "Google") SetEngine("Google", "https://www.google.com/search?q=");
        if (selection == "Startpage") SetEngine("Startpage", "https://www.startpage.com/search?q=");
        if (selection == "Qwant") SetEngine("Qwant", "https://www.qwant.com/?q=");
        if (selection == "Yahoo!") SetEngine("Yahoo!", "https://search.yahoo.com/search?p=");
    }

    private static void SetEngine(string EngineFriendlyName, string SearchUrl)
    {
        SettingsHelper.SetSetting("EngineFriendlyName", EngineFriendlyName);
        SettingsHelper.SetSetting("SearchUrl", SearchUrl);
        Globals.SearchUrl = SearchUrl;
    }

    public static List<string> SearchEngines = new()
    {
            "Ask",
            "Baidu",
            "Bing",
            "Brave Search",
            "DuckDuckGo",
            "Ecosia",
            "Google",
            "Startpage",
            "Qwant",
            "Yahoo!"
    };
}
