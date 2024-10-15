namespace Bluebird.Core;

public static class UrlHelper
{
    private static readonly Regex UrlMatch = new("^(http(s)?:\\/\\/.)?(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{1,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)$", RegexOptions.Singleline);
    private static readonly Regex IPMatch = new("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(?::([0-9]{1,5}))?$", RegexOptions.Singleline);
    public static string GetInputType(string input)
    {
        string type;
        
        if (input.StartsWith("http://") || input.StartsWith("https://") || input.StartsWith("edge://"))
        {
            type = "url";
        }
        else if (UrlMatch.IsMatch(input) || IPMatch.IsMatch(input))
        {
            type = "urlNOProtocol";
        }
        else
        {
            type = "searchquery";
        }
        return type;
    }
}
