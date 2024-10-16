namespace Bluebird.Core;

public static class WindowManager
{
    public static void EnterFullScreen(bool fs)
    {
        var view = ApplicationView.GetForCurrentView();
        if (fs)
        {
            view.TryEnterFullScreenMode();
        }
        else
        {
            view.ExitFullScreenMode();
        }
    }
}
