namespace Bluebird.Core;

public class ExceptionHelper
{
    public static async void ThrowFullError(Exception ex)
    {
        await UI.ShowDialog("Exception thrown!", $"{ex.Message}\n\n{ex.Source}\n\n{ex}\n\n{ex.StackTrace}");
    }
}
