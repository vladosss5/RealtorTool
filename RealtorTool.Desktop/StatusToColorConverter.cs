using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace RealtorTool.Desktop;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string status)
        {
            return status switch
            {
                "Активно" => "#4CAF50",
                "Продано" => "#F44336", 
                "Снято" => "#FF9800",
                "Черновик" => "#9E9E9E",
                "На проверке" => "#2196F3",
                _ => "#757575"
            };
        }
        return "#757575";
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}