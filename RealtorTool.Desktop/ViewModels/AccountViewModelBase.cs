using RealtorTool.Core.Enums;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels;

public class AccountViewModelBase : PageViewModelBase
{
    protected readonly IAccountingService AccountingService;

    public AccountViewModelBase(IAccountingService accountingService)
    {
        AccountingService = accountingService;
    }

    // Свойства для удобства привязки в XAML
    public bool IsSystemAdministrator => AccountingService.HasRole(UserRole.SystemAdministrator);
    public bool IsSystem => AccountingService.HasRole(UserRole.System);
    public bool IsAdministrator => AccountingService.HasRole(UserRole.Administrator);
    public bool IsRealtor => AccountingService.HasRole(UserRole.Realtor);
    
    public bool IsAdminOrHigher => AccountingService.HasAnyRole(
        UserRole.SystemAdministrator, 
        UserRole.Administrator
    );
}