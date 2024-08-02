using Bluebird.Modules.QRCodeGen;

namespace Bluebird.Core;

public static class QRCodeHelper
{
    public static async Task<byte[]> GenerateQRCodeFromUrlAsync(string url)
    {
        //Create raw qr code data
        QRCodeGenerator qrGenerator = new();
        QRCodeData qrCodeData = qrGenerator.CreateQrCode(url, QRCodeGenerator.ECCLevel.M);

        //Create byte/raw bitmap qr code
        BitmapByteQRCode qrCodeBmp = new(qrCodeData);
        byte[] qrCodeImageBmp = qrCodeBmp.GetGraphic(20);
        
        return qrCodeImageBmp;
    }

    public static async Task<BitmapImage> ConvertBitmapBytesToImage(byte[] bytes)
    {

        var image = new BitmapImage();
        using (InMemoryRandomAccessStream stream = new())
        {
            using (DataWriter writer = new(stream.GetOutputStreamAt(0)))
            {
                writer.WriteBytes(bytes);
                await writer.StoreAsync();
            }
            await image.SetSourceAsync(stream);
        }
        return image;
    }
}
