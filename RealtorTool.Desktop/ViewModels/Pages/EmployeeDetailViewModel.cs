using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Data.Context;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class EmployeeDetailViewModel : PageViewModelBase, IParameterReceiver
{
    private readonly DataContext _context;
    
    [Reactive] public Employee Employee { get; set; }
    [Reactive] public bool CanFire { get; set; }
    [Reactive] public bool CanRecover { get; set; }
    
    public ICommand ResetPasswordCommand { get; }
    public ICommand FireCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand RecoverCommand { get; }

    public EmployeeDetailViewModel(DataContext context)
    {
        _context = context;

        ResetPasswordCommand = ReactiveCommand.CreateFromTask(ResetPasswordImpl);
        FireCommand = ReactiveCommand.CreateFromTask(FireImpl);
        DeleteCommand = ReactiveCommand.CreateFromTask(DeleteImpl);
        RecoverCommand = ReactiveCommand.CreateFromTask(RecoverImpl);

        GetDataFromMessageBus();
    }
    
    public void ReceiveParameter(object parameter)
    {
        if (parameter is string listingId)
        {
            _ = LoadEmployeeAsync(listingId);
        }
    }
    
    private async Task ResetPasswordImpl()
    {
        throw new NotImplementedException();
    }

    private async Task FireImpl()
    {
        try
        {
            Employee.Fired = true;
            CanFire = false;
            CanRecover = true;

            _context.Attach(Employee);
        
            await _context.SaveChangesAsync();
            
            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Сотрудник уволен", ButtonEnum.Ok)
                .ShowAsync();
        }
        catch (Exception e)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", e.InnerException.Message, ButtonEnum.Ok)
                .ShowAsync();
        }
    }
    
    private async Task RecoverImpl()
    {
        try
        {
            Employee.Fired = false;
            CanFire = true;
            CanRecover = false;

            _context.Attach(Employee);
        
            await _context.SaveChangesAsync();
            
            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Сотрудник восстановлен", ButtonEnum.Ok)
                .ShowAsync();
        }
        catch (Exception e)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", e.InnerException.Message, ButtonEnum.Ok)
                .ShowAsync();
        }
    }

    private async Task DeleteImpl()
    {
        try
        {
            Employee.IsDeleted = true;

            _context.Attach(Employee);
        
            await _context.SaveChangesAsync();
            
            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Сотрудник удалён", ButtonEnum.Ok)
                .ShowAsync();
        }
        catch (Exception e)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", e.InnerException.Message, ButtonEnum.Ok)
                .ShowAsync();
        }
    }

    private async Task LoadEmployeeAsync(string listingId)
    {
        try
        {
            Employee = await _context.Employees
                .Include(x => x.Photo)
                .Include(x => x.Role)
                .FirstAsync(l => l.Id == listingId);

            if (Employee != null)
            {
                Title = $"Сотрудник №{Employee.Id} - {Employee.FullName}";
                CanFire = !Employee.Fired;
                CanRecover = Employee.Fired;
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Не удалось загрузить сотрудника: {ex.Message}")
                .ShowAsync();
        }
    }

    private void GetDataFromMessageBus()
    {
        MessageBus.Current
            .Listen<string>("ListingDetailsId")
            .SelectMany(async listingId =>
            {
                await LoadEmployeeAsync(listingId);
                return Unit.Default;
            })
            .Subscribe();
    }
}