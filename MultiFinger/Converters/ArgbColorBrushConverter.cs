using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MultiFinger.Converters
{
    public class ArgbColorBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string colorValue)
            {
                //This gives us an array of 4 strings each representing a number in text form.
                var splitString = colorValue.Split(';');

                //converts the array of 4 strings in to an array of 4 bytes.
                var splitBytes = splitString.Select(item => byte.Parse(item)).ToArray();

                return new SolidColorBrush(Color.FromArgb(splitBytes[0], splitBytes[1], splitBytes[2], splitBytes[3]));
            }

            return null;   
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
