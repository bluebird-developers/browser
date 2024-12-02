namespace Bluebird.Core;

public class SearchEngineHelper
{
    public static void SetSearchEngine(SearchEngine engine)
    {
        SettingsHelper.SetSetting("EngineFriendlyName", engine.EngineFriendlyName);
        SettingsHelper.SetSetting("SearchUrl", engine.SearchUrl);
        SearchUrl = engine.SearchUrl;
    }

    public static List<SearchEngine> SearchEngines =
    [
        new SearchEngine("Ask", "https://www.ask.com/web?q="),
        new SearchEngine("Baidu", "https://www.baidu.com/s?ie=utf-8&f=8&rsv_bp=1&rsv_idx=1&tn=baidu&wd="),
        new SearchEngine("Bing", "https://www.bing.com?q="),
        new SearchEngine("Brave Search", "https://search.brave.com/search?q="),
        new SearchEngine("DuckDuckGo", "https://www.duckduckgo.com?q="),
        new SearchEngine("Ecosia", "https://www.ecosia.org/search?q="),
        new SearchEngine("Google", "https://www.google.com/search?q="),
        new SearchEngine("Startpage", "https://www.startpage.com/search?q="),
        new SearchEngine("Qwant", "https://www.qwant.com/?q="),
        new SearchEngine("Yahoo!", "https://search.yahoo.com/search?p="),
        new SearchEngine("Yandex", "https://yandex.com/search/?text=")
    ];
}
