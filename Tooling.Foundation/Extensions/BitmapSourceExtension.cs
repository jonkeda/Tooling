using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Tooling.Extensions
{
    public static class BitmapSourceExtension
    {
        public static BitmapImage ToBitmapImage(this BitmapSource bitmapSource)
        {
            if (bitmapSource == null)
            {
                return null;
            }
            BitmapImage bImg;
            MemoryStream memoryStream;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            using (memoryStream = new MemoryStream())
            {
                bImg = new BitmapImage();

                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));
                encoder.Save(memoryStream);

                bImg.BeginInit();
                bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
                bImg.EndInit();

                memoryStream.Close();
            }
            return bImg;
        }

        public static bool CanClip(this BitmapSource bitmapSource,
            Int32Rect r)
        {
            return r.X >= 0
                && r.Y >= 0
                && r.Width >= 1
                && r.Width < bitmapSource.Width
                && r.Height >= 1
                && r.Height < bitmapSource.Height;
        }

        public static BitmapImage Clip(this BitmapSource bitmapSource,
            Rectangle r)
        {
            int top = Math.Max(r.Y, 0);
            int left = Math.Max(r.X, 0);
            int bottom = Math.Min(r.Y + r.Height, (int)bitmapSource.Height);
            int right = Math.Min(r.X + r.Width, (int)bitmapSource.Width);

            Int32Rect rect = new Int32Rect(left, top, right - left, bottom - top);
            if (CanClip(bitmapSource, rect))
            {
                CroppedBitmap bm = new CroppedBitmap(bitmapSource, rect);

                return bm.ToBitmapImage();
            }
            return null;
        }

        public static BitmapImage Clip(this BitmapSource bitmapSource,
            double left, double top, double width, double height)
        {
            Int32Rect rect = new Int32Rect((int)left, (int)top, (int)width, (int)height);
            if (CanClip(bitmapSource, rect))
            {
                CroppedBitmap bm = new CroppedBitmap(bitmapSource, rect);
                return bm.ToBitmapImage();
            }
            return null;
        }

        public static BitmapImage ResizeImage(this BitmapSource source, int size)
        {
            BitmapImage bi;
            MemoryStream memoryStream;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            using (memoryStream = new MemoryStream())
            {
                bi = new BitmapImage();

                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(memoryStream);

                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnDemand;
                bi.CreateOptions = BitmapCreateOptions.DelayCreation;
                if (source.Height > source.Width)
                {
                    bi.DecodePixelHeight = size;
                }
                else
                {
                    bi.DecodePixelWidth = size;
                }
                bi.StreamSource = new MemoryStream(memoryStream.ToArray());
                bi.EndInit();

                memoryStream.Close();
            }
            return bi;
        }

        public static BitmapImage ResizeImage(this BitmapSource source, int width, int height)
        {
            BitmapImage bi;
            MemoryStream memoryStream;
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            using (memoryStream = new MemoryStream())
            {
                bi = new BitmapImage();

                encoder.Frames.Add(BitmapFrame.Create(source));
                encoder.Save(memoryStream);

                bi.BeginInit();
                bi.CacheOption = BitmapCacheOption.OnDemand;
                bi.CreateOptions = BitmapCreateOptions.DelayCreation;
                bi.DecodePixelHeight = height;
                bi.DecodePixelWidth = width;
                bi.StreamSource = new MemoryStream(memoryStream.ToArray());
                bi.EndInit();

                memoryStream.Close();

            }
            return bi;
        }

    }
}