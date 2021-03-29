using System;
using System.Drawing;

namespace Tooling.Foundation.Extensions
{
    public static class ImageHelper
    {
        //private static IImageHelper _helper;

        //public static IImageHelper Helper
        //{
        //    get
        //    {
        //        if (_helper == null
        //            && HelperType != null)
        //        {
        //            _helper = Activator.CreateInstance(HelperType) as IImageHelper;
        //        }
        //        return _helper;
        //    }
        //}

        public static Type HelperType { get; set; }

        //public static Rectangle? TemplateMatch(BitmapSource baseImage, BitmapSource image, double threshold)
        //{
        //    return Helper.TemplateMatch(baseImage, image, threshold);
        //}

        //public static Rectangle TemplateMatch(ITestRunContext context, BitmapSource image, double timeout,
        //    int? imageNumber, double threshold, CaptureType captureType, bool setFocus)
        //{
        //    if (setFocus)
        //    {
        //        context.ActiveTestApplication.SetFocus();
        //    }
        //    return Helper.TemplateMatch(context, image, timeout, imageNumber, threshold, captureType);
        //}

        public static Bitmap CaptureScreen(this Rectangle bounds)
        {
            // create the bitmap to copy the screen shot to
            Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);

            // now copy the screen image to the graphics device from the bitmap
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.CopyFromScreen(bounds.Location, Point.Empty, bounds.Size);
            }

            return bitmap;
        }

        public static Bitmap CaptureBitmap(Rectangle rectangle, IntPtr handle)

        {
            Size windowSize = rectangle.Size;
            
            Bitmap windowBitmap = GetWindowBitmap(windowSize, handle);
            //if (OsInformation.MajorVersion == 6 && OsInformation.MinorVersion == 2)
            //{
            //    windowBitmap = GetWindowBitmap(windowSize, handle);
            //}
            if (IsBadImage(windowBitmap))
            {
                windowBitmap?.Dispose();
                windowBitmap = GetWindowBitmapBitBlt(rectangle, handle);
                if (IsBadImage(windowBitmap))
                {
                    windowBitmap?.Dispose();
                    windowBitmap =  GetWindowBitmapBitBltOnDesktop(rectangle);
                }
            }
            return windowBitmap;
        }

        private static bool IsBadImage(Bitmap bmp)
        {
            if (bmp == null)
            {
                return true;
            }
            int height = 3 * bmp.Height / 8;
            int num = 5 * bmp.Height / 8;
            int width = 3 * bmp.Width / 8;
            int width1 = 5 * bmp.Width / 8;
            int argb = Color.Black.ToArgb();
            int num1 = (num - height) * (width1 - width);
            int num2 = (int)((double)num1 * 0.9);
            int num3 = num1 - num2;
            int num4 = 0;
            int num5 = 0;
            for (int i = height; i < num; i++)
            {
                for (int j = width; j < width1; j++)
                {
                    if (bmp.GetPixel(j, i).ToArgb() == argb)
                    {
                        num4++;
                        if (num4 == num2)
                        {
                            return true;
                        }
                    }
                    else
                    {
                        num5++;
                        if (num5 == num3)
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        private static Bitmap GetWindowBitmapBitBlt(Rectangle rectangle, IntPtr intPtr)
        {
            NativeMethodsEx.RECT rect;
            Bitmap bitmap;
            bool flag = false;
            NativeMethodsEx.RECT rECT1 = new NativeMethodsEx.RECT()
            {
                Left = rectangle.Left,
                Top = rectangle.Top,
                Right = rectangle.Right,
                Bottom = rectangle.Bottom
            };
            NativeMethodsEx.RECT x = rECT1;
            if (x.Height == 0 || x.Width == 0)
            {
                return null;
            }
            if (flag)
            {
                Point point = new Point(x.Left, x.Top);
                Point point1 = new Point(x.Right, x.Bottom);
                NativeMethodsEx.User32.ClientToScreen(intPtr, ref point);
                NativeMethodsEx.User32.ClientToScreen(intPtr, ref point1);
                x.Left = point.X;
                x.Top = point.Y;
                x.Right = point1.X;
                x.Bottom = point1.Y;
            }
            IntPtr desktopWindow = NativeMethodsEx.User32.GetDesktopWindow();
            NativeMethodsEx.RECT rECT2 = new NativeMethodsEx.RECT();
            NativeMethodsEx.User32.GetWindowRect(desktopWindow, ref rECT2);
            if (!NativeMethodsEx.User32.IntersectRect(out rect, ref rECT2, ref x))
            {
                rect = x;
            }
            if (NativeMethodsEx.User32.IsRectEmpty(ref rect))
            {
                return null;
            }
            int width = rect.Width;
            int height = rect.Height;
            IntPtr zero = IntPtr.Zero;
            IntPtr windowDC = IntPtr.Zero;
            IntPtr zero1 = IntPtr.Zero;
            try
            {
                Bitmap bitmap1 = null;
                windowDC = NativeMethodsEx.User32.GetWindowDC(intPtr);
                zero = NativeMethodsEx.Gdi32.CreateCompatibleDC(windowDC);
                zero1 = NativeMethodsEx.Gdi32.CreateCompatibleBitmap(windowDC, width, height);
                if (zero1 != IntPtr.Zero)
                {
                    int num = 0;
                    int num1 = 0;
                    IntPtr intPtr1 = NativeMethodsEx.Gdi32.SelectObject(zero, zero1);
                    NativeMethodsEx.Gdi32.BitBlt(zero, 0, 0, width, height, windowDC, num, num1, 80478240);
                    NativeMethodsEx.Gdi32.SelectObject(zero, intPtr1);
                    bitmap1 = Image.FromHbitmap(zero1);
                }
                bitmap = bitmap1;
            }
            finally
            {
                if (windowDC != IntPtr.Zero)
                {
                    NativeMethodsEx.User32.ReleaseDC(intPtr, windowDC);
                }
                if (zero != IntPtr.Zero)
                {
                    NativeMethodsEx.Gdi32.DeleteDC(zero);
                }
                if (zero1 != IntPtr.Zero)
                {
                    NativeMethodsEx.Gdi32.DeleteObject(zero1);
                }
            }
            return bitmap;
        }

        private static Bitmap GetWindowBitmapBitBltOnDesktop(Rectangle rectangle)
        {
            Bitmap bitmap;

            NativeMethodsEx.RECT rEct = new NativeMethodsEx.RECT()
            {
                Left = rectangle.Left,
                Top = rectangle.Top,
                Right = rectangle.Right,
                Bottom = rectangle.Bottom
            };
            NativeMethodsEx.RECT rECT1 = rEct;
            if (rECT1.Height == 0 || rECT1.Width == 0)
            {
                return null;
            }
            IntPtr zero = IntPtr.Zero;
            IntPtr dC = IntPtr.Zero;
            IntPtr intPtr = IntPtr.Zero;
            try
            {
                Bitmap bitmap1 = null;
                dC = NativeMethodsEx.User32.GetDC(IntPtr.Zero);
                zero = NativeMethodsEx.Gdi32.CreateCompatibleDC(dC);
                intPtr = NativeMethodsEx.Gdi32.CreateCompatibleBitmap(dC, rECT1.Width, rECT1.Height);
                if (intPtr != IntPtr.Zero)
                {
                    IntPtr intPtr1 = NativeMethodsEx.Gdi32.SelectObject(zero, intPtr);
                    NativeMethodsEx.Gdi32.BitBlt(zero, 0, 0, rECT1.Width, rECT1.Height, dC, rECT1.Left, rECT1.Top, 80478240);
                    NativeMethodsEx.Gdi32.SelectObject(zero, intPtr1);
                    bitmap1 = Image.FromHbitmap(intPtr);
                }
                bitmap = bitmap1;
            }
            finally
            {
                if (dC != IntPtr.Zero)
                {
                    NativeMethodsEx.User32.ReleaseDC(IntPtr.Zero, dC);
                }
                if (zero != IntPtr.Zero)
                {
                    NativeMethodsEx.Gdi32.DeleteDC(zero);
                }
                if (intPtr != IntPtr.Zero)
                {
                    NativeMethodsEx.Gdi32.DeleteObject(intPtr);
                }
            }
            return bitmap;
        }

        private static Bitmap GetWindowBitmap(Size windowSize, IntPtr handle)
        {
            Bitmap bitmap = null;
            IntPtr dC = NativeMethodsEx.User32.GetDC(handle);
            IntPtr intPtr = NativeMethodsEx.Gdi32.CreateCompatibleDC(dC);
            IntPtr intPtr1 = NativeMethodsEx.Gdi32.CreateCompatibleBitmap(dC, windowSize.Width, windowSize.Height);
            if (intPtr1 == IntPtr.Zero)
            {
                NativeMethodsEx.User32.ReleaseDC(handle, dC);
                NativeMethodsEx.Gdi32.DeleteDC(intPtr);
                return null;
            }
            IntPtr intPtr2 = NativeMethodsEx.Gdi32.SelectObject(intPtr, intPtr1);
            if (NativeMethodsEx.User32.PrintWindow(handle, intPtr, 0))
            {
                NativeMethodsEx.Gdi32.SelectObject(intPtr, intPtr2);
                bitmap = Image.FromHbitmap(intPtr1);
            }
            NativeMethodsEx.User32.ReleaseDC(handle, dC);
            NativeMethodsEx.Gdi32.DeleteDC(intPtr);
            NativeMethodsEx.Gdi32.DeleteObject(intPtr1);
            return bitmap;
        }

        private static Bitmap GetWindowBitmap(Rectangle rectangle, IntPtr handle)
        {
            NativeMethodsEx.RECT rECT;
            Bitmap bitmap;
            IntPtr dC = NativeMethodsEx.User32.GetDC(handle);
            IntPtr intPtr = handle;
            bool flag = false;
            //Rectangle rectangle = this.Rectangle;
            NativeMethodsEx.RECT rECT1 = new NativeMethodsEx.RECT
            {
                Left = rectangle.Left,
                Top = rectangle.Top,
                Right = rectangle.Right,
                Bottom = rectangle.Bottom
            };
            NativeMethodsEx.RECT x = rECT1;
            if (x.Height == 0 || x.Width == 0)
            {
                return null;
            }
            if (flag)
            {
                Point point = new Point(x.Left, x.Top);
                Point point1 = new Point(x.Right, x.Bottom);
                NativeMethodsEx.User32.ClientToScreen(intPtr, ref point);
                NativeMethodsEx.User32.ClientToScreen(intPtr, ref point1);
                x.Left = point.X;
                x.Top = point.Y;
                x.Right = point1.X;
                x.Bottom = point1.Y;
            }
            IntPtr desktopWindow = NativeMethodsEx.User32.GetDesktopWindow();
            NativeMethodsEx.RECT rECT2 = new NativeMethodsEx.RECT();
            NativeMethodsEx.User32.GetWindowRect(desktopWindow, ref rECT2);
            if (!NativeMethodsEx.User32.IntersectRect(out rECT, ref rECT2, ref x))
            {
                rECT = x;
            }
            if (NativeMethodsEx.User32.IsRectEmpty(ref rECT))
            {
                return null;
            }
            int width = rECT.Width;
            int height = rECT.Height;
            IntPtr zero = IntPtr.Zero;
            IntPtr windowDC = IntPtr.Zero;
            IntPtr zero1 = IntPtr.Zero;
            try
            {
                Bitmap bitmap1 = null;
                windowDC = NativeMethodsEx.User32.GetWindowDC(intPtr);
                zero = NativeMethodsEx.Gdi32.CreateCompatibleDC(windowDC);
                zero1 = NativeMethodsEx.Gdi32.CreateCompatibleBitmap(windowDC, width, height);
                if (zero1 != IntPtr.Zero)
                {
                    int num = 0;
                    int num1 = 0;
                    IntPtr intPtr1 = NativeMethodsEx.Gdi32.SelectObject(zero, zero1);
                    NativeMethodsEx.Gdi32.BitBlt(zero, 0, 0, width, height, windowDC, num, num1, 80478240);
                    NativeMethodsEx.Gdi32.SelectObject(zero, intPtr1);
                    bitmap1 = Image.FromHbitmap(zero1);
                }
                bitmap = bitmap1;
            }
            finally
            {
                if (windowDC != IntPtr.Zero)
                {
                    NativeMethodsEx.User32.ReleaseDC(intPtr, windowDC);
                }
                if (zero != IntPtr.Zero)
                {
                    NativeMethodsEx.Gdi32.DeleteDC(zero);
                }
                if (zero1 != IntPtr.Zero)
                {
                    NativeMethodsEx.Gdi32.DeleteObject(zero1);
                }
            }
            return bitmap;
        }

        public static bool CanClone(this Bitmap bitmap, Rectangle rec)
        {
            if (bitmap == null)
            {
                return false;
            }
            return rec.Height > 0
                   && rec.Width > 0
                   && new Rectangle(Point.Empty, bitmap.Size).Contains(rec);
        }
    }
}