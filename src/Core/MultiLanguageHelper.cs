namespace Bluebird.Core;

public static class MultiLanguageHelper
{
    public static void OverrideLanguage(string langCode)
    {
        Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = langCode;
    }

    public static string GetPreferredLanguage()
    {
        return Windows.Globalization.ApplicationLanguages.Languages[0];
    }
}
