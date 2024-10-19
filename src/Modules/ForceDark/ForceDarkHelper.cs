using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Bluebird.Modules.ForceDark;

public class ForceDarkHelper
{
    public static string jscript { private set; get; }
    public static async Task<string> GetForceDarkScriptAsync()
    {
        if (jscript == null)
        {
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Modules/ForceDark/Assets/forcedark.min.js"));
            jscript = await FileIO.ReadTextAsync(file);
        }
        return jscript;
    }
}