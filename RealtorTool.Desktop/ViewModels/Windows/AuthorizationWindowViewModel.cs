using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Data;
using RealtorTool.Desktop.Views;
using RealtorTool.Services.Interfaces;
using MainWindow = RealtorTool.Desktop.Views.Windows.MainWindow;

namespace RealtorTool.Desktop.ViewModels;

/// <summary>
/// VM Окна авторизации.
/// </summary>
public class AuthorizationWindowViewModel : ViewModelBase
{
    private readonly DataContext _context;
    private readonly IWindowService _windowService;
    
    /// <summary>
    /// Поле логина.
    /// </summary>
    [Reactive] public string? Login { get; set; }
        
    /// <summary>
    /// Поле пароля.
    /// </summary>
    [Reactive] public string? Password { get; set; }
    
    /// <summary>
    /// Бинд для кнопки авторизации.
    /// </summary>
    public ReactiveCommand<Window, Unit> Auth { get; }
    
    /// <summary>
    /// Конструктор.
    /// </summary>
    public AuthorizationWindowViewModel(
        DataContext context, 
        IWindowService windowService)
    {
        _context = context;
        _windowService = windowService;
        Auth = ReactiveCommand.CreateFromTask<Window>(AuthAsync);
    }

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public AuthorizationWindowViewModel()
    { }

    /// <summary>
    /// Метод проверки авторизации.
    /// </summary>
    private async Task AuthAsync(Window currentWindow)
    {
        var authData = await _context.AuthorizationData.FirstOrDefaultAsync(x => x.Login == Login);

        if (authData == null || authData.Password != Password)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Логин или пароль не совпадают", ButtonEnum.Ok)
                .ShowAsync();
            
            return;
        }
            
        MessageBus.Current.SendMessage(authData.Id, "CurrentAuthId");
        
        _windowService.ShowWindow<MainWindow>();
        currentWindow.Close();
    }
}