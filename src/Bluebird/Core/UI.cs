namespace Bluebird.Core;

public class UI
{
    public static async Task ShowDialog(string title, string content)
    {
        ContentDialog dialog = new()
        {
            Title = title,
            Content = content,
            PrimaryButtonText = "Okay",
            DefaultButton = ContentDialogButton.Primary
        };

        await dialog.ShowAsync();
    }

    public static async Task<ContentDialogResult> ShowDialogWithAction(string title, string content, string PrimaryBtnText, string CloseBtnText)
    {
        ContentDialog dialog = new()
        {
            Title = title,
            Content = content,
            PrimaryButtonText = PrimaryBtnText,
            CloseButtonText = CloseBtnText,
            DefaultButton = ContentDialogButton.Primary
        };

        var result = await dialog.ShowAsync();
        return result;
    }
}
