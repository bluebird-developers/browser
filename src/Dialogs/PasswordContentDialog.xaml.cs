namespace Bluebird.Dialogs;

public sealed partial class PasswordContentDialog : ContentDialog
{
    public PasswordContentDialog() => this.InitializeComponent();

    private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
    {
        string password = AppLockPasswordBox.Password;
        if (password != string.Empty)
            PasswordHelper.SaveCredential("User", password);
    }
}
