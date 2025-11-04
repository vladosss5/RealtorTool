using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RealtorTool.Desktop.Converters;

public class TrueToFalseConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is bool boolValue)
        {
            return boolValue ? "Поиск..." : "Обновить поиск";
        }
        return "Обновить поиск";
    }
    
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}