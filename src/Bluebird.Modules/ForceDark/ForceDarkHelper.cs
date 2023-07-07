using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Bluebird.Modules.ForceDark;

public class ForceDarkHelper
{
    public static async Task<string> GetForceDarkScriptAsync()
    {
        StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Bluebird.Modules/ForceDark/Assets/forcedark.min.js"));
        string jscript = await FileIO.ReadTextAsync(file);
        return jscript;
    }
}