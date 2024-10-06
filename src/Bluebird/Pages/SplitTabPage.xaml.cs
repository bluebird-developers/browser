namespace Bluebird.Pages;

public sealed partial class SplitTabPage : Page
{
    public SplitTabPage()
    {
        this.InitializeComponent();
        WebTabCreationParams webTabCreationParams = new()
        {
            IsSplitTab = true
        };
        LeftFrame.Navigate(typeof(WebViewPage), webTabCreationParams);
        RightFrame.Navigate(typeof(WebViewPage), webTabCreationParams);
    }

    public void CloseWebViews()
    {
        (LeftFrame.Content as WebViewPage).WebViewControl.Close();
        (RightFrame.Content as WebViewPage).WebViewControl.Close();
    }
}
