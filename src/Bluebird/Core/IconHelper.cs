namespace Bluebird.Core;

public static class IconHelper
{
    public static muxc.IconSource ConvFavURLToIconSource(string url)
    {
        try
        {
            Uri faviconUrl = new(url);
            muxc.BitmapIconSource iconsource = new() { UriSource = faviconUrl, ShowAsMonochrome = false };
            return iconsource;
        }
        catch
        {
            muxc.IconSource iconsource = new muxc.SymbolIconSource() { Symbol = Symbol.Document };
            return iconsource;
        }
    }
}