// Data/Utils/ImageHelper.cs
using System.Drawing.Imaging;

namespace SportClubApp.Data.Utils
{
    public static class ImageHelper
    {
        public static byte[] ConvertirImagenABytes(Image imagen, ImageFormat formato = null)
        {
            if (imagen == null) return null;

            using var ms = new MemoryStream();
            imagen.Save(ms, formato ?? ImageFormat.Jpeg);
            return ms.ToArray();
        }

        public static Image ConvertirBytesAImagen(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return null;

            using var ms = new MemoryStream(bytes);
            return Image.FromStream(ms);
        }

        public static Image RedimensionarImagen(Image imgOriginal, int anchoMax, int altoMax)
        {
            if (imgOriginal == null) return null;

            if (imgOriginal.Width <= anchoMax && imgOriginal.Height <= altoMax)
                return imgOriginal;

            double ratioX = (double)anchoMax / imgOriginal.Width;
            double ratioY = (double)altoMax / imgOriginal.Height;
            double ratio = Math.Min(ratioX, ratioY);

            int anchoNuevo = (int)(imgOriginal.Width * ratio);
            int altoNuevo = (int)(imgOriginal.Height * ratio);

            var imgRedimensionada = new Bitmap(anchoNuevo, altoNuevo);

            using (var graphics = Graphics.FromImage(imgRedimensionada))
            {
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.DrawImage(imgOriginal, 0, 0, anchoNuevo, altoNuevo);
            }

            return imgRedimensionada;
        }

        public static string ObtenerTipoMIME(string extension)
        {
            return extension.ToLower() switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                _ => "image/jpeg"
            };
        }
    }
}
