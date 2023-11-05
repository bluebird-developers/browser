using System.IO;

namespace Bluebird.Modules.QRCodeGen.Framework4._0Methods;

class Stream4Methods
{
    public static void CopyTo(Stream input, Stream output)
    {
        byte[] buffer = new byte[16 * 1024];
        int bytesRead;
        while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            output.Write(buffer, 0, bytesRead);
        }
    }
}
