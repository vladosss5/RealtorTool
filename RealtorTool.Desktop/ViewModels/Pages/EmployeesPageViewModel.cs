using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.Models.DbEntities;
using RealtorTool.Data.Context;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class EmployeesPageViewModel : PageViewModelBase
{
    private readonly DataContext _context;
    private readonly IAccountingService _accountingService;

    [Reactive] public Employee NewEmployee { get; set; } = new();
    
    [Reactive] public string Password { get; set; }
    
    public ICommand CreateEmployee { get; }

    public EmployeesPageViewModel(
        DataContext context, 
        IAccountingService accountingService)
    {
        _context = context;
        _accountingService = accountingService;
        CreateEmployee = ReactiveCommand.CreateFromTask(CreateEmployeeAsync);
    }

    private async Task CreateEmployeeAsync()
    {
        var passwordHashAndSalt = _accountingService.HashPassword(Password);

        NewEmployee.PasswordHash = passwordHashAndSalt.Hash;
        NewEmployee.Salt = passwordHashAndSalt.Salt;
        
        await _context.Employees.AddAsync(NewEmployee);
        await _context.SaveChangesAsync();
    }
}