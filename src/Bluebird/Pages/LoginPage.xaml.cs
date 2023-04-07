namespace Bluebird.Pages;

public sealed partial class LoginPage : Page
{
    public LoginPage()
    {
        this.InitializeComponent();
    }

    private void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox.Password != null)
            {
                // TODO
            }
        }
    }
}
