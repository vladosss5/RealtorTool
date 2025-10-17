using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RealtorTool.Desktop.Converters;

public class ObjectIsNotNullConverter : IValueConverter
{
    public static ObjectIsNotNullConverter Instance { get; } = new();

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value != null;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        // Обычно в односторонних конвертерах этот метод не используется
        throw new NotSupportedException();
    }
}