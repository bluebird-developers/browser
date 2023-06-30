namespace Bluebird.FRE.Pages;

/// <summary>
/// The first FRE page
/// It welcomes the user to their newly downloaded browser
/// </summary>
public sealed partial class FREPage1 : Page
{
    public FREPage1()
    {
        this.InitializeComponent();
    }

    private void GetStartedButton_Click(object sender, RoutedEventArgs e)
    {
        //Frame.Navigate(typeof(FREPage2));
    }
}
