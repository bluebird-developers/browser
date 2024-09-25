namespace Bluebird.Core;

public static class NewTabHelper
{
    public static string GetRandomWallpaper()
    {
        Random random = new();
        int number = random.Next(1, 10);
        string randomImagePath = $"ms-appx:///Assets/Backgrounds/Background_{number}.jpg";
        return randomImagePath;
    }
}
