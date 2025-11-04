using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace RealtorTool.Desktop.Converters;

public class MatchScoreToBackgroundConverter : IValueConverter
{
    public static MatchScoreToBackgroundConverter Instance { get; } = new();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is int score)
        {
            return score >= 90 ? new SolidColorBrush(Color.FromArgb(20, 0, 255, 0)) : // Светло-зеленый
                score >= 70 ? new SolidColorBrush(Color.FromArgb(20, 255, 165, 0)) : // Светло-оранжевый
                score >= 50 ? new SolidColorBrush(Color.FromArgb(20, 0, 0, 255)) : // Светло-синий
                new SolidColorBrush(Color.FromArgb(10, 255, 0, 0)); // Светло-красный
        }
        return Brushes.Transparent;
    }
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}