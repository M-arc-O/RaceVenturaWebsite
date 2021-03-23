using QRCoder;
using System.Drawing;
using System.IO;

namespace RaceVenturaAPI.Helpers
{
    public static class RaceQrCodeHelper
    {
        public static MemoryStream CreateQrCodes(string txtQRCode)
        {
            QRCodeGenerator qrCodeGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrCodeGenerator.CreateQrCode(txtQRCode, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            MemoryStream stream = new MemoryStream();
            qrCodeImage.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            return stream;
        }
    }
}
