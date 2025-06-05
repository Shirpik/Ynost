using System.Globalization;
using System.Windows;
using System.Windows.Data;
namespace Ynost.Converters
{
    public class InverseBooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var boolValue = false;

            if (value is bool)
            {
                boolValue = (bool)value;
            }
            // Инвертируем значение перед преобразованием в Visibility
            return !boolValue ? Visibility.Visible : Visibility.Collapsed;
        }
        
        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}