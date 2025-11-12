using System;
using System.Globalization;
using RealtorTool.Core.Enums;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.Converters;

public class RoleToVisibilityConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IAccountingService accountingService && parameter is string roleString)
        {
            if (Enum.TryParse<UserRole>(roleString, out var requiredRole))
            {
                return accountingService.HasRole(requiredRole);
            }
        }
        
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}