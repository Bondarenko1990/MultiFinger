using System.Globalization;
using System.Windows.Data;

namespace MultiFinger.Converters
{
    public class DoubleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
                return d.ToString("F2", culture); 

            return value?.ToString() ?? "";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), out double result))
                return result;

            return 0.0;
        }
    }
}
