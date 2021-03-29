using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Tooling.Mio;

namespace Tooling.Foundation.UI.Converters
{
    public class VirtualFileImageSourceConverter : IValueConverter
    {
        private static readonly ImageSourceConverter Converter = new ImageSourceConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value is VirtualFile file)
            {
                return file.ToImageSource();
            }

            if (Converter.CanConvertFrom(value.GetType()))
            {
                return Converter.ConvertFrom(value);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
