using System;
using System.Globalization;
using Avalonia.Data;
using Avalonia.Data.Converters;

namespace RealtorTool.Desktop.Converters;

public class DateTimeFormatConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is DateTime dateTime)
        {
            return dateTime.ToString("dd-MM-yyyy HH-mm");
        }
        
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo? culture)
    {
        if (value is string dateString && DateTime.TryParseExact(dateString, "dd-MM-yyyy HH-mm", 
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
        {
            return result;
        }
        
        return BindingOperations.DoNothing;
    }
}