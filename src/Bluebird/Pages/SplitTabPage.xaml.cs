namespace Bluebird.Pages;

public sealed partial class SplitTabPage : Page
{
    public SplitTabPage()
    {
        this.InitializeComponent();
        LeftFrame.Navigate(typeof(WebViewPage));
        RightFrame.Navigate(typeof(WebViewPage));
    }

    public void CloseWebViews()
    {
        (LeftFrame.Content as WebViewPage).WebViewControl.Close();
        (RightFrame.Content as WebViewPage).WebViewControl.Close();
    }
}
