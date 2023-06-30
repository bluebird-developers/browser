using System.Text.RegularExpressions;

namespace Bluebird.Core;

public class UrlHelper
{
    public static string GetInputType(string input)
    {
        string type;
        Regex UrlMatch = new("^(http(s)?:\\/\\/.)?(www\\.)?[-a-zA-Z0-9@:%._\\+~#=]{2,256}\\.[a-z]{2,6}\\b([-a-zA-Z0-9@:%_\\+.~#?&//=]*)$", RegexOptions.Singleline);
        if (UrlMatch.IsMatch(input))
        {
            type = "url";
        }
        else
        {
            type = "searchquery";
        }
        return type;
    }
}
