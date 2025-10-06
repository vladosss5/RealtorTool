using System;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.Models.DbModels;
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
                CurrentPerson = x;
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

    // private void LoadPersonData()
    // {
    //     CurrentPerson = _context.Persons.FirstOrDefault(x => x.Id == _authorizationData.Id);
    //
    //     if (CurrentPerson == default)
    //         return;
    //     
    //     FName = CurrentPerson?.FName!;
    //     SName = CurrentPerson?.SName!;
    //     LName = CurrentPerson?.LName!;
    //     PhoneNumber = CurrentPerson?.PhoneNumber!;
    //     EMail = CurrentPerson?.EMail!;
    // }
}