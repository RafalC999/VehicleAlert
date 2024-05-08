using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VehicleAlert
{
    public class ProgressForegroundConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double progress = (double)value;
            SolidColorBrush foreground = new SolidColorBrush(Colors.Green);

            if (progress >= 90)
            {
                foreground = new SolidColorBrush(Colors.Red);
            }
            else if (progress >= 75)
            {
                foreground = new SolidColorBrush(Colors.Orange);
            }
            else if (progress < 75)
            {
                foreground = new SolidColorBrush(Colors.Green);
            }

            return foreground;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

