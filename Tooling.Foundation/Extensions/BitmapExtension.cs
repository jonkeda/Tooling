using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Media.Imaging;
using Tooling.Threading;
using Color = System.Windows.Media.Color;

namespace Tooling.Extensions
{
    public static class BitmapExtension
    {
        // Specify a pixel format.
        public const PixelFormat PIXEL_FORMAT = PixelFormat.Format32bppArgb;

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }

            BitmapImage bImg = null;
            try
            {
                ThreadDispatcher.Invoke(() =>
                {
                    MemoryStream ms = new MemoryStream();
                    bitmap.Save(ms, ImageFormat.Png);
                    bImg = new BitmapImage();

                    ms.Position = 0;

                    bImg.BeginInit();
                    bImg.StreamSource = ms;
                    bImg.EndInit();
                });
                return bImg;
            }
            catch
            {
                return null;
            }
        }

        public static BitmapImage LoadImageFromResource(Assembly assembly, string resourceName)
        {
            try
            {
                Stream stream = assembly.GetManifestResourceStream(resourceName);

                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.StreamSource = stream;
                bmi.EndInit();

                return bmi;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static BitmapImage LoadImage(string fileName)
        {
            if (File.Exists(fileName))
            {
                BitmapImage bmi = new BitmapImage();
                bmi.BeginInit();
                bmi.CacheOption = BitmapCacheOption.OnLoad;
                bmi.UriSource = new Uri(fileName, UriKind.Absolute);
                bmi.EndInit();
                return bmi;
            }
            return null;
        }

        public static void SaveImage(this BitmapSource bitmap, string fileName)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (FileStream filestream = new FileStream(fileName, FileMode.Create))
            {
                encoder.Save(filestream);
            }
        }

        public static byte[] ToByteArray(this BitmapSource bitmap)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmap));

            using (MemoryStream filestream = new MemoryStream())
            {
                encoder.Save(filestream);
                return filestream.GetBuffer();
            }
        }

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowDC(IntPtr hwnd);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("gdi32.dll")]
        public static extern ulong BitBlt(IntPtr hDestDC, int x, int y,
            int nWidth, int nHeight, IntPtr hSrcDC,
            int xSrc, int ySrc, int dwRop);

        public static BitmapImage GetScreenBitmap(double x, double y, double width, double height)
        {
            int ix, iy, iw, ih;
            ix = Convert.ToInt32(x);
            iy = Convert.ToInt32(y);
            iw = Convert.ToInt32(width);
            ih = Convert.ToInt32(height);
            try
            {
                Bitmap myImage = new Bitmap(iw, ih);
                Graphics gr1 = Graphics.FromImage(myImage);
                IntPtr dc1 = gr1.GetHdc();
                IntPtr dc2 = GetWindowDC(GetForegroundWindow());
                BitBlt(dc1, ix, iy, iw, ih, dc2, ix, iy, 13369376);
                gr1.ReleaseHdc(dc1);
                return ToBitmapImage(myImage);
            }
            catch
            { }
            return null;
        }

        public static Bitmap ToBitmap(this BitmapSource bitmapImage)
        {
            if (bitmapImage == null)
            {
                return null;
            }
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                Bitmap bitmap = new Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public static void NonTransparantToOneColor(this Bitmap bmp, Color color)
        {
            // Lock the bitmap's bits.
            Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite, PIXEL_FORMAT);

            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            // int numBytes = bmp.Width * bmp.Height * 3; 
            int numBytes = bmpData.Stride * bmp.Height;
            byte[] rgbValues = new byte[numBytes];

            // Copy the RGB values into the array.
            Marshal.Copy(ptr, rgbValues, 0, numBytes);

            // Manipulate the bitmap
            for (int counter = 0; counter < rgbValues.Length; counter += 4)
            {
                if (rgbValues[counter + 3] != 0)
                {
                    rgbValues[counter] = color.B;
                    rgbValues[counter + 1] = color.G;
                    rgbValues[counter + 2] = color.R;
                    rgbValues[counter + 3] = color.A;
                }
            }

            // Copy the RGB values back to the bitmap
            Marshal.Copy(rgbValues, 0, ptr, numBytes);

            // Unlock the bits.
            bmp.UnlockBits(bmpData);
        }

        #region Compare

        public static Rectangle GetBounds(this Bitmap bitmap)
        {
            return new Rectangle(new Point(), bitmap.Size);
        }


        public static bool CompareImage(int[][] oldArray, int[][] newArray, int matchPercentage, int blockSize,
            byte colorLimit)
        {
            int height = oldArray.Length;
            int width = oldArray[0].Length;

            for (int y = 0; y < height; y += blockSize)
            {
                for (int x = 0; x < width; x += blockSize)
                {
                    int count;
                    int yc;
                    int xc;
                    CompareBlock(oldArray, newArray, blockSize, x, y, colorLimit,
                            out count, out xc, out yc);
                    if (xc > 0
                        && yc > 0
                        && ((count * 100) / (xc * yc)) > matchPercentage)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public static void CompareBlock(int[][] oldArray, int[][] newArray, int blockSize,
            int xStart, int yStart,
            byte colorLimit,
            out int count, out int xc, out int yc)
        {
            int height = newArray.Length;
            int width = newArray[0].Length;
            count = 0;

            int xEnd = xStart + blockSize;
            int yEnd = yStart + blockSize;

            yc = 0;
            xc = 0;
            for (int y = yStart; y < height && y < yEnd; y++)
            {
                int[] oldLine = oldArray[y];
                int[] newLine = newArray[y];

                yc++;
                xc = 0;
                for (int x = xStart; x < width && x < xEnd; x++)
                {
                    xc++;
                    if (!CompareColor(oldLine[x], newLine[x], colorLimit))
                    {
                        count++;
                    }
                }
            }
        }

        public static bool CompareColor(int oldCi, int newCi, byte colorLimit)
        {
            System.Drawing.Color oldC = System.Drawing.Color.FromArgb(oldCi);
            System.Drawing.Color newC = System.Drawing.Color.FromArgb(newCi);

            return CompareColor(oldC.A, newC.A, colorLimit)
                   && CompareColor(oldC.R, newC.R, colorLimit)
                   && CompareColor(oldC.G, newC.G, colorLimit)
                   && CompareColor(oldC.B, newC.B, colorLimit);
        }

        private static bool CompareColor(byte oldCi, byte newCi, byte colorLimit)
        {
            return Math.Abs(oldCi - newCi) <= colorLimit;
        }

        #endregion

        #region to from

        public static int[][] BitmapToIntArray(this Bitmap bitmap, PixelFormat pixelFormat)
        {
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            BitmapData bitmapData = bitmap.LockBits(rect, ImageLockMode.ReadOnly, pixelFormat);
            int[][] oldArray = BitmapToIntArray(bitmapData);
            bitmap.UnlockBits(bitmapData);
            return oldArray;
        }

        public static int[][] BitmapToIntArray(BitmapData bmpData)
        {
            // Get the address of the first line.
            IntPtr ptr = bmpData.Scan0;

            // Declare an array to hold the bytes of the bitmap. 
            int width = bmpData.Width;
            int height = bmpData.Height;
            int[][] lines = new int[height][];

            // Copy the RGB values into the array.
            for (int i = 0; i < height; i++)
            {
                int[] line = new int[width];
                lines[i] = line;
                Marshal.Copy(ptr, line, 0, width);
                ptr += width * 4;
            }

            return lines;
        }

        #endregion


        #region Mark

        public static void MarkImage(int[][] oldArray, int[][] newArray, Bitmap compare, int matchPercentage, int blockSize,
            int borderX, int borderY,
            byte colorLimit)
        {
            int height = oldArray.Length;
            int width = oldArray[0].Length;

            for (int y = 0; y < height; y += blockSize)
            {
                for (int x = 0; x < width; x += blockSize)
                {
                    MarkBlock(oldArray, newArray, compare, matchPercentage, blockSize, x, y, borderX, borderY, colorLimit);
                }
            }
        }

        private static void MarkBlock(int[][] oldArray, int[][] newArray, Bitmap compare,
            int matchPercentage, int blockSize,
            int xStart, int yStart,
            int borderX, int borderY,
            byte colorLimit)
        {
            int height = newArray.Length;
            int width = newArray[0].Length;
            int count = 0;

            int xEnd = xStart + blockSize;
            int yEnd = yStart + blockSize;

            int yc = 0;
            int xc = 0;
            for (int y = yStart; y + borderY < height && y < yEnd; y++)
            {
                int[] oldLine = oldArray[y];
                int[] newLine = newArray[y + borderY];
                yc++;
                xc = 0;
                for (int x = xStart; x + borderX < width && x < xEnd; x++)
                {
                    xc++;
                    if (!CompareColor(oldLine[x], newLine[x + borderX], colorLimit))
                    {
                        count++;
                    }
                }
            }
            if (xc == 0)
            {
                return;
            }
            if (((count * 100) / (xc * yc)) > matchPercentage)
            {
                using (Graphics g = Graphics.FromImage(compare))
                {
                    for (int y = yStart; y + borderY < height && y < yEnd; y++)
                    {
                        int[] oldLine = oldArray[y];
                        int[] newLine = newArray[y + borderY];
                        for (int x = xStart; x + borderX < width && x < xEnd; x++)
                        {
                            if (!CompareColor(oldLine[x], newLine[x + borderX], colorLimit))
                            {
                                g.FillRectangle(Brushes.Red, x, y, 1, 1);

                                //compareArray[y][x] = int.MaxValue;
                                //0xFFFF0000;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region ToImageSource

        public static BitmapImage ToImageSource(this Bitmap bitmap)
        {
            if (bitmap == null)
            {
                return null;
            }

            ImageFormat imgFormat = ImageFormat.Png;

            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, imgFormat);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        #endregion


        public static Bitmap ScaleImage(this Bitmap clone, int scale)
        {
            if (clone == null)
            {
                return null;
            }

            Bitmap large = new Bitmap(clone.Width * scale, clone.Height * scale);
            using (Graphics g = Graphics.FromImage(large))
            {
                g.DrawImage(clone, 0, 0, large.Width, large.Height);

                return large;
            }
        }
    }
}