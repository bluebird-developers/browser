namespace Bluebird.Core;

public class WebView2CreationError(string ErrorMsg)
{
    public string ErrorMsg { get; private set; } = ErrorMsg;
}
