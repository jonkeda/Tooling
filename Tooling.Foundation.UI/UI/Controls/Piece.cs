using System.Windows.Media;

namespace Tooling.Foundation.UI.Controls
{
    public class Piece : IPieceValue, IPieceBackgroundColor, IPieceForegroundColor
    {
        public Piece()
        {
        }

        public Piece(double value, Brush backgroundColor)
        {
            Value = value;
            BackgroundColor = backgroundColor;
        }

        public Piece(double value, Brush foregroundColor, Brush backgroundColor)
        {
            Value = value;
            ForegroundColor = foregroundColor;
            BackgroundColor = backgroundColor;
        }

        public double Value { get; set; }
        public Brush ForegroundColor { get; set; }
        public Brush BackgroundColor { get; set; }

        public double GetValue()
        {
            return Value;
        }

        public Brush GetBackgroundColor()
        {
            return BackgroundColor;
        }

        public Brush GetForegroundColor()
        {
            return ForegroundColor;
        }
    }
}