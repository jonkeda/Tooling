using System;
using System.Globalization;
using System.Windows.Data;

namespace Tooling.Foundation.UI.Converters
{
    public class EnumSourceConverter : BaseConverter, IValueConverter
    {
        public static readonly IValueConverter Instance = new EnumSourceConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is Type type
                && type.IsEnum)
            {
                return Enum.GetValues(type);
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}