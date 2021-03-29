using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace Tooling.Foundation.UI.Controls
{
    public class ImageCapture : Image
    {
        #region Properties

        public static readonly DependencyProperty IsCapturingProperty = DependencyProperty.Register(
            "IsCapturing", typeof(bool), typeof(ImageCapture), new PropertyMetadata(default(bool)));

        public bool IsCapturing
        {
            get { return (bool) GetValue(IsCapturingProperty); }
            set { SetValue(IsCapturingProperty, value); }
        }

        public static readonly DependencyProperty CaptureLeftProperty = DependencyProperty.Register(
            nameof(CaptureLeft), typeof(double?), typeof(ImageCapture), 
            new FrameworkPropertyMetadata(default(double?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double? CaptureLeft
        {
            get { return (double?) GetValue(CaptureLeftProperty); }
            set { SetValue(CaptureLeftProperty, value); }
        }

        public static readonly DependencyProperty CaptureTopProperty = DependencyProperty.Register(
            nameof(CaptureTop), typeof(double?), typeof(ImageCapture), 
            new FrameworkPropertyMetadata(default(double?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double? CaptureTop
        {
            get { return (double?) GetValue(CaptureTopProperty); }
            set { SetValue(CaptureTopProperty, value); }
        }

        public static readonly DependencyProperty CaptureWidthProperty = DependencyProperty.Register(
            nameof(CaptureWidth), typeof(double?), typeof(ImageCapture), 
            new FrameworkPropertyMetadata(default(double?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public double? CaptureWidth
        {
            get { return (double?) GetValue(CaptureWidthProperty); }
            set { SetValue(CaptureWidthProperty, value); }
        }

        public static readonly DependencyProperty CaptureHeightProperty = DependencyProperty.Register(
            nameof(CaptureHeight), typeof(double?), typeof(ImageCapture),
            new FrameworkPropertyMetadata(default(double?), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        private Point _location;
        private double? _copyLeft;
        private double? _copyTop;
        private double? _copyHeight;
        private double? _copyWidth;

        public double? CaptureHeight
        {
            get { return (double?) GetValue(CaptureHeightProperty); }
            set { SetValue(CaptureHeightProperty, value); }
        }
        #endregion

        #region Methods
        
        protected override void OnPreviewMouseMove(MouseEventArgs e)
        {
            base.OnPreviewMouseMove(e);
            if (IsCapturing)
            {
                SetLocation(e.GetPosition(this));
            }
        }

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            IsCapturing = CaptureMouse();

            _copyLeft = CaptureLeft;
            _copyTop = CaptureTop;
            _copyHeight = CaptureHeight;
            _copyWidth = CaptureWidth;

            _location = e.GetPosition(this);
            SetLocation(e.GetPosition(this));
        }

        protected override void OnPreviewMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonUp(e);
            Point newLocation = e.GetPosition(this);
            if (_location == newLocation)
            {
                CaptureLeft = _copyLeft;
                CaptureTop = _copyTop;
                CaptureHeight = _copyHeight;
                CaptureWidth = _copyWidth;
            }
            else
            {
                SetLocation(newLocation);
            }
            ReleaseMouseCapture();
            IsCapturing = false;
        }

        private void SetLocation(Point newLocation)
        {
            BitmapImage bmi = Source as BitmapImage;
            double factorX = 1;
            double factorY = 1;
            if (bmi != null)
            {
                factorX = bmi.DpiX / 96;
                factorY = bmi.DpiY / 96;
            }

            if (_location.X < newLocation.X)
            {
                CaptureLeft = Math.Floor(_location.X * factorX);
                CaptureWidth = Math.Ceiling((newLocation.X - _location.X) * factorX);
            }
            else
            {
                CaptureLeft = Math.Floor(newLocation.X * factorX);
                CaptureWidth = Math.Ceiling((_location.X - newLocation.X) * factorX);
            }

            if (_location.Y < newLocation.Y)
            {
                CaptureTop = Math.Floor(_location.Y * factorY);
                CaptureHeight = Math.Ceiling((newLocation.Y - _location.Y) * factorY);
            }
            else
            {
                CaptureTop = Math.Floor(newLocation.Y * factorY);
                CaptureHeight = Math.Ceiling((_location.Y - newLocation.Y) * factorY);
            }
        }

        #endregion
    }

    public class ImageEx : Image
    {
        public ICommand MouseLeftButtonDownCommand
        {
            get { return (ICommand)GetValue(MouseLeftButtonDownCommandProperty); }
            set { SetValue(MouseLeftButtonDownCommandProperty, value); }
        }
        public static readonly DependencyProperty MouseLeftButtonDownCommandProperty =
            DependencyProperty.Register("MouseLeftButtonDownCommand", typeof(ICommand), typeof(ImageEx), new PropertyMetadata(null));

        public ICommand MouseLeftButtonUpCommand
        {
            get { return (ICommand)GetValue(MouseLeftButtonUpCommandProperty); }
            set { SetValue(MouseLeftButtonUpCommandProperty, value); }
        }
        public static readonly DependencyProperty MouseLeftButtonUpCommandProperty =
            DependencyProperty.Register("MouseLeftButtonUpCommand", typeof(ICommand), typeof(ImageEx), new PropertyMetadata(null));

        public ICommand MouseMoveCommand
        {
            get { return (ICommand)GetValue(MouseMoveCommandProperty); }
            set { SetValue(MouseMoveCommandProperty, value); }
        }
        public static readonly DependencyProperty MouseMoveCommandProperty =
            DependencyProperty.Register("MouseMoveCommand", typeof(ICommand), typeof(ImageEx), new PropertyMetadata(null));

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            MouseMoveCommand?.Execute(new CanvasExEventArgs(this, e));
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            MouseLeftButtonDownCommand?.Execute(new CanvasExEventArgs(this, e));
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            MouseLeftButtonUpCommand?.Execute(new CanvasExEventArgs(this, e));
        }
    }
}
