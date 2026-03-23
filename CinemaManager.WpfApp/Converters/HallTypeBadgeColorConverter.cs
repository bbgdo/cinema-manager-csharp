using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace CinemaManager.Wpf.Converters;

public class HallTypeBadgeColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return (value as string) switch
        {
            "IMAX" => new SolidColorBrush(Color.FromRgb(0xC0, 0x39, 0x2B)),
            "VIP"  => new SolidColorBrush(Color.FromRgb(0xD4, 0x89, 0x0A)),
            "3D"   => new SolidColorBrush(Color.FromRgb(0x6C, 0x3F, 0xC5)),
            _      => new SolidColorBrush(Color.FromRgb(0x21, 0x76, 0xAE)),
        };
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
