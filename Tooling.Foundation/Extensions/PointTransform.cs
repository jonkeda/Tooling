using System.Drawing;
using System.Windows;
using System.Windows.Media;
using Point = System.Windows.Point;

namespace Tooling.Extensions
{
    public static class PointTransform
    {
        private static bool _initialized;
        public static Matrix TransformFromDevice { get; private set; }
        public static Matrix TransformToDevice { get; private set; }

        public static void Initialize(System.Windows.Media.Visual visual)
        {
            if (_initialized)
            {
                return;
            }
            _initialized = true;
            Set(visual);
        }

        public static void Set(System.Windows.Media.Visual visual)
        {
            PresentationSource x = PresentationSource.FromVisual(visual);
            if (x?.CompositionTarget != null)
            {
                TransformFromDevice = x.CompositionTarget.TransformFromDevice;
                TransformToDevice = x.CompositionTarget.TransformToDevice;
            }
        }

        public static Point FromDevice(this Point point)
        {
            if (_initialized)
            {
                return TransformFromDevice.Transform(point);
            }
            return point;
        }

        public static Point ToDevice(this Point point)
        {
            if (_initialized)
            {
                return TransformToDevice.Transform(point);
            }
            return point;
        }

        //public static System.Drawing.Point ToDevice(this System.Drawing.Point? point)
        //{
        //    if (point.HasValue)
        //    {
        //        return point.Value.ToDevice();
        //    }
        //    return System.Drawing.Point.Empty;
        //}

        //public static System.Drawing.Point ToDevice(this System.Drawing.Point point)
        //{
        //    if (_initialized)
        //    {
        //        return TransformToDevice.Transform(point.ToPoint()).ToPoint();
        //    }
        //    return point;
        //}

        //public static PointF ToDevice(this PointF point)
        //{
        //    if (_initialized)
        //    {
        //        return TransformToDevice.Transform(point.ToPoint()).ToPoint();
        //    }
        //    return point;
        //}

        //public static System.Drawing.Point FromDevice(this System.Drawing.Point point)
        //{
        //    if (_initialized)
        //    {
        //        return TransformFromDevice.Transform(point.ToPoint()).ToPoint();
        //    }
        //    return point;
        //}

        //public static System.Drawing.Size FromDevice(this System.Drawing.Size size)
        //{
        //    if (_initialized)
        //    {
        //        return TransformFromDevice.Transform(size.ToPoint().ToPoint()).ToSize();
        //    }
        //    return size;
        //}

        //public static Rectangle ToDevice(this Rectangle rectangle)
        //{
        //    if (_initialized)
        //    {
        //        System.Drawing.Point location = ToDevice(rectangle.Location);
        //        System.Drawing.Size size = ToDevice(rectangle.Size.ToPoint()).ToSize();
        //        return new Rectangle(location, size);
        //    }
        //    return rectangle;
        //}

        //public static RectangleF ToDevice(this RectangleF rectangle)
        //{
        //    if (_initialized)
        //    {
        //        PointF location = ToDevice(rectangle.Location);
        //        SizeF size = ToDevice(rectangle.Size.ToPointF()).ToSizeF();

        //        return new RectangleF(location, size);
        //    }
        //    return rectangle;
        //}

        //public static System.Drawing.Point Middle(this Rectangle rectangle)
        //{
        //    return new System.Drawing.Point(rectangle.X + rectangle.Width / 2, rectangle.Y + rectangle.Height / 2);
        //}


        //public static Rectangle FromDevice(this Rectangle rectangle)
        //{
        //    if (_initialized)
        //    {
        //        System.Drawing.Point location = FromDevice(rectangle.Location);
        //        System.Drawing.Size size = FromDevice(rectangle.Size.ToPoint()).ToSize();

        //        return new Rectangle(location, size);
        //    }
        //    return rectangle;
        //}

    }
}
