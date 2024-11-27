namespace Bluebird.Core;

public static class UrlHelper
{
    private static readonly Regex IPMatch = new("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)(?::([0-9]{1,5}))?$", RegexOptions.Singleline | RegexOptions.Compiled);

    public static string GetInputType(string input)
    {
        bool IsValidUrl = Uri.IsWellFormedUriString(input, UriKind.RelativeOrAbsolute);

        if (IsValidUrl || IPMatch.IsMatch(input))
        {
            if (input.StartsWith("http://") || input.StartsWith("https://") || input.StartsWith("edge://"))
            {
                return "url";
            }
            return "urlNOProtocol";

        }
        if (!IsValidUrl)
        {
            return "searchquery";
        }
        return null;
    }
}
