using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbModels.PersonModels;
using RealtorTool.Data;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class MyProfilePageViewModel : PageViewModelBase
{
    private AuthorizationData _authorizationData;

    private readonly DataContext _context;
    
    [Reactive] public Person? CurrentPerson { get; set; }
    
    [Reactive] public string FName { get; set; }
    
    [Reactive] public string SName { get; set; }
    
    [Reactive] public string LName { get; set; }
    
    [Reactive] public string PhoneNumber { get; set; }
    
    [Reactive] public string EMail { get; set; }
    
    public ICommand SaveChanges { get; }
    
    public MyProfilePageViewModel(DataContext context)
    {
        _context = context;
        
        MessageBus.Current
            .Listen<AuthorizationData>("CurrentAuthId")
            .Subscribe(x => 
            {
                _authorizationData = x;
                
                if (_authorizationData != default)
                    LoadPersonData();
            });

        SaveChanges = ReactiveCommand.CreateFromTask(SaveChangesProfileDataAsync);
    }

    private async Task SaveChangesProfileDataAsync()
    {
        if (CurrentPerson == default)
            return;
        
        CurrentPerson.EMail = EMail;
        CurrentPerson.PhoneNumber = PhoneNumber;

        _context.Update(CurrentPerson);
        await _context.SaveChangesAsync();
    }

    private void LoadPersonData()
    {
        CurrentPerson = _context.Persons.FirstOrDefault(x => x.Id == _authorizationData.Id);

        if (CurrentPerson == default)
            return;
        
        FName = CurrentPerson?.FName!;
        SName = CurrentPerson?.SName!;
        LName = CurrentPerson?.LName!;
        PhoneNumber = CurrentPerson?.PhoneNumber!;
        EMail = CurrentPerson?.EMail!;
    }
}