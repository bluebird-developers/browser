using FREPages = Bluebird.FRE.Pages;

namespace Bluebird.FRE;

/// <summary>
/// The wrapper for all FRE pages
/// </summary>
public sealed partial class FREWrapper : Page
{
    public FREWrapper()
    {
        this.InitializeComponent();
        FREWrapperFrame.Navigate(typeof(FREPages.FREPage1));
    }
}
