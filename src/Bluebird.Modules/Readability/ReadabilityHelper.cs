using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Bluebird.Modules.Readability;

public class ReadabilityHelper
{
    public static async Task<string> GetReadabilityScriptAsync()
    {
        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Bluebird.Modules/Readability/Assets/readability.js"));
        string jscript = await FileIO.ReadTextAsync(file);
        return jscript;
    }
}