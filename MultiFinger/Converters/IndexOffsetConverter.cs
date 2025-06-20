﻿using System.Globalization;
using System.Windows.Data;

namespace MultiFinger.Converters
{
    public class IndexOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index)
                return index + 1;

            return 1;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (int.TryParse(value?.ToString(), out int number))
                return number - 1;

            return 0;
        }
    }


}
