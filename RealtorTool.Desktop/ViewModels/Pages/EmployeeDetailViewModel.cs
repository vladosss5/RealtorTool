using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
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

    public EmployeeDetailViewModel(DataContext context)
    {
        _context = context;

        GetDataFromMessageBus();
    }
    
    public void ReceiveParameter(object parameter)
    {
        if (parameter is string listingId)
        {
            _ = LoadEmployeeAsync(listingId);
        }
    }

    private async Task LoadEmployeeAsync(string listingId)
    {
        try
        {
            Employee = (await _context.Employees
                .Include(x => x.Photo)
                .Include(x => x.Role)
                .FirstAsync(l => l.Id == listingId));

            if (Employee != null)
            {
                Title = $"Сотрудник №{Employee.Id} - {Employee.FullName}";
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