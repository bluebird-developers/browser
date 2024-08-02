namespace Bluebird.Core;

public static class IconHelper
{
    public static muxc.IconSource ConvFavURLToIconSource(string url)
    {
        try
        {
            if (url != string.Empty)
            {
                Uri faviconUrl = new(url);
                muxc.BitmapIconSource iconsource = new() { UriSource = faviconUrl, ShowAsMonochrome = false };
                return iconsource;
            }
            else
            {
                return GetFallbackFavicon();
            }
        }
        catch
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine("URL:" + url);
#endif
            return GetFallbackFavicon();
        }
    }

    private static muxc.IconSource GetFallbackFavicon()
    {
        muxc.IconSource iconsource = new muxc.SymbolIconSource() { Symbol = Symbol.Document };
        return iconsource;
    }
}