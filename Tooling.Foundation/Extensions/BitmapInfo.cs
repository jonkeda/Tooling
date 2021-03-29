using System.Drawing;

namespace Tooling.Extensions
{
    public class BitmapInfo
    {
        public BitmapInfo(Bitmap image)
        {
            Matrix = image.BitmapToIntArray(BitmapExtension.PIXEL_FORMAT); ;
            Height = image.Height;
            Width = image.Width;
        }

        public int[][] Matrix { get; }
        public int Height { get; }
        public int Width { get; }
    }
}