using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class MyProfilePageViewModel : PageViewModelBase
{
    private readonly DataContext _context;
    private readonly IAccountingService _accountingService;
    
    [Reactive] public Employee? CurrentPerson { get; set; }
    [Reactive] public string OldPassword { get; set; }
    [Reactive] public string NewPassword { get; set; }
    [Reactive] public string RepeatPassword { get; set; }
    
    public ICommand SaveChanges { get; }
    public ICommand ChangePassword { get; }
    
    public MyProfilePageViewModel(
        DataContext context, 
        IAccountingService accountingService)
    {
        _context = context;
        _accountingService = accountingService;

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
        ChangePassword = ReactiveCommand.CreateFromTask(ChangePasswordAsync);
    }

    private async Task SaveChangesProfileDataAsync()
    {
        if (CurrentPerson == default)
            return;

        _context.Update(CurrentPerson);
        await _context.SaveChangesAsync();
    }
    
    private async Task ChangePasswordAsync()
    {
        if (CurrentPerson == null)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Пользователь не авторизован!", ButtonEnum.Ok)
                .ShowAsync();
            return;
        }
            
            
        CurrentPerson = await _context.Employees
            .Include(x => x.Role)
            .Include(x => x.Photo)
            .FirstAsync(x => x.Id == CurrentPerson!.Id);

        var passwordIsRight =
            _accountingService.VerifyPassword(OldPassword, CurrentPerson.PasswordHash, CurrentPerson.Salt!);
        
        if (!passwordIsRight)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Старый пароль не совпадает", ButtonEnum.Ok)
                .ShowAsync();
            return;
        }

        if (NewPassword != RepeatPassword)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Новые пароли не совпадают", ButtonEnum.Ok)
                .ShowAsync();
            return;
        }
        
        var newPasswordHash = _accountingService.HashPassword(NewPassword);

        CurrentPerson.PasswordHash = newPasswordHash.Hash;
        CurrentPerson.Salt = newPasswordHash.Salt;
        
        _context.Attach(CurrentPerson);
        await _context.SaveChangesAsync();
        
        await MessageBoxManager
            .GetMessageBoxStandard("Успех", "Пароль изменён!", ButtonEnum.Ok)
            .ShowAsync();

        ClearForm();
    }

    private void ClearForm()
    {
        OldPassword = null;
        NewPassword = null;
        RepeatPassword = null;
    }
}