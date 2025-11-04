using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Data.Context;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class EmployeesPageViewModel : PageViewModelBase
{
    private readonly DataContext _context;
    private readonly IAccountingService _accountingService;
    
    [Reactive] public ObservableCollection<Employee> Employees { get; set; }
    [Reactive] public Employee NewEmployee { get; set; } = new();
    [Reactive] public string Password { get; set; }
    
    public ICommand CreateEmployee { get; }

    public EmployeesPageViewModel(
        DataContext context, 
        IAccountingService accountingService)
    {
        _context = context;
        _accountingService = accountingService;

        LoadData();
        
        CreateEmployee = ReactiveCommand.CreateFromTask(CreateEmployeeAsync);
    }

    private void LoadData()
    {
        Employees = new ObservableCollection<Employee>(
            _context.Employees
                .Include(x => x.Role)
                .Include(x => x.Photo)
                .ToList());
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