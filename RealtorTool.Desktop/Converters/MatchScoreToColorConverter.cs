using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace RealtorTool.Desktop.Converters;

public class MatchScoreToColorConverter : IValueConverter
{
    public static MatchScoreToColorConverter Instance { get; } = new();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is decimal score)
        {
            return score switch
            {
                >= 90 => new SolidColorBrush(Colors.Green),      // Идеальное совпадение
                >= 70 => new SolidColorBrush(Colors.Orange),     // Хорошее совпадение  
                >= 50 => new SolidColorBrush(Colors.Yellow),     // Возможное совпадение
                _ => new SolidColorBrush(Colors.LightGray)       // Слабое совпадение
            };
        }
        return new SolidColorBrush(Colors.LightGray);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}