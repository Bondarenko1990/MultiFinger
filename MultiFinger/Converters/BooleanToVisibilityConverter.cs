using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace MultiFinger.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        private static string InverseParameter = "invert";

        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            bool returnValue;

            if (parameter as string == InverseParameter)
            {
                returnValue = !(bool)value;
            }
            else
            {
                returnValue = (bool)value;
            }

            return returnValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            throw new NotImplementedException();
        }
    }
}
