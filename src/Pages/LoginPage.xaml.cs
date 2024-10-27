namespace Bluebird.Pages;

public sealed partial class LoginPage : Page
{
    public LoginPage() => this.InitializeComponent();

    private async void PasswordBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        if (e.Key == VirtualKey.Enter)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            if (passwordBox.Password != null)
            {
                string password = PasswordHelper.GetCredential();
                if (passwordBox.Password == password)
                    Frame.Navigate(typeof(MainPage), null, new DrillInNavigationTransitionInfo());
                else
                    await UI.ShowDialog("Error", "The password is incorrect");
            }
        }
    }
}
