using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Data.Context;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class MyProfilePageViewModel : PageViewModelBase
{
    private readonly DataContext _context;
    
    [Reactive] public Employee? CurrentPerson { get; set; }
    
    public ICommand SaveChanges { get; }
    
    public MyProfilePageViewModel(DataContext context)
    {
        _context = context;
        
        MessageBus.Current
            .Listen<Employee>("CurrentAuth")
            .Subscribe(x => 
            {
                if (x != null)
                {
                    CurrentPerson = _context.Employees
                        .Include(x => x.Role)
                        .Include(x => x.Photo)
                        .FirstOrDefault(p => p.Id ==x.Id);   
                }
            });

        SaveChanges = ReactiveCommand.CreateFromTask(SaveChangesProfileDataAsync);
    }

    private async Task SaveChangesProfileDataAsync()
    {
        if (CurrentPerson == default)
            return;

        _context.Update(CurrentPerson);
        await _context.SaveChangesAsync();
    }
}