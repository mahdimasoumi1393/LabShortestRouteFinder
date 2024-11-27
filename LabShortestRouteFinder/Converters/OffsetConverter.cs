using System;
using System.Globalization;
using System.Windows.Data;

namespace LabShortestRouteFinder.Converters
{
    public class OffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int originalValue && parameter is string offsetString && double.TryParse(offsetString, out double offset))
            {
                return originalValue + offset;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
