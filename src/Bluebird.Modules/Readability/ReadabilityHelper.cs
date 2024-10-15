using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Bluebird.Modules.Readability;

public class ReadabilityHelper
{
    public static string jscript { private set; get; }

    public static async Task<string> GetReadabilityScriptAsync()
    {
        if (jscript == null)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Bluebird.Modules/Readability/Assets/readability.js"));
            jscript = await FileIO.ReadTextAsync(file);
        }
        return jscript;
    }
}