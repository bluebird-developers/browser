namespace Bluebird.Core;

public static class IconHelper
{
    public static Microsoft.UI.Xaml.Controls.IconSource ConvFavURLToIconSource(string url)
    {
        try
        {
            Uri faviconUrl = new(url);
            Microsoft.UI.Xaml.Controls.BitmapIconSource iconsource = new() { UriSource = faviconUrl, ShowAsMonochrome = false };
            return iconsource;
        }
        catch
        {
            Microsoft.UI.Xaml.Controls.IconSource iconsource = new Microsoft.UI.Xaml.Controls.SymbolIconSource() { Symbol = Symbol.Document };
            return iconsource;
        }
    }
}