using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CinemaManager.Wpf.Converters;

public class PosterImageConverter : IValueConverter
{
    public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is not string fileName || string.IsNullOrWhiteSpace(fileName))
            return null;

        try
        {
            var uri = new Uri($"pack://application:,,,/Assets/Posters/{fileName}");
            return new BitmapImage(uri);
        }
        catch
        {
            return null;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
        throw new NotSupportedException();
}
