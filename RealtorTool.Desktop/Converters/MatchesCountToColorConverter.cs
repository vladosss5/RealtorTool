using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace RealtorTool.Desktop.Converters;

public class MatchesCountToColorConverter : IValueConverter
{
    public static MatchesCountToColorConverter Instance { get; } = new();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int count)
        {
            return count > 0 ? Brushes.Green : Brushes.Red;
        }
        return Brushes.Black;
    }
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}