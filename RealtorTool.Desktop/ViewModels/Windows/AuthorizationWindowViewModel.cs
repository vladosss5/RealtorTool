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
    private readonly IWindowService _windowService;
    private readonly IAuthorizationService _authorizationService;
    
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
        IWindowService windowService,
        IAuthorizationService authorizationService)
    {
        _windowService = windowService;
        _authorizationService = authorizationService;
        Auth = ReactiveCommand.CreateFromTask<Window>(AuthAsync);
    }

    /// <summary>
    /// Конструктор по умолчанию
    /// </summary>
    public AuthorizationWindowViewModel(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Метод проверки авторизации.
    /// </summary>
    private async Task AuthAsync(Window currentWindow)
    {
        var authPerson = await _authorizationService.LoginAsync(Login, Password);

        if (authPerson == null)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Логин или пароль не совпадают", ButtonEnum.Ok)
                .ShowAsync();
            
            return;
        }
        
        _windowService.ShowWindow<MainWindow>();
        MessageBus.Current.SendMessage(authPerson, "CurrentAuth");
        currentWindow.Close();
    }
}