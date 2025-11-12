using System;
using System.Globalization;
using System.Linq;
using RealtorTool.Core.Enums;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.Converters;

public class AnyRoleToVisibilityConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IAccountingService accountingService && parameter is string rolesString)
        {
            var requiredRoles = rolesString.Split(',')
                .Select(role => Enum.Parse<UserRole>(role.Trim()))
                .ToArray();
                
            return accountingService.HasAnyRole(requiredRoles);
        }
        
        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}