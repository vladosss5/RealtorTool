using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.Views.Windows;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.Services.Implementations;

/// <inheritdoc />
public class WindowService : IWindowService
{
    private readonly IServiceProvider _serviceProvider;
    private Window? _currentWindow;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public WindowService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    /// <inheritdoc />
    public void ShowWindow<TWindow>() where TWindow : Window
    {
        var window = _serviceProvider.GetRequiredService<TWindow>();
        window.Show();
        _currentWindow = window;
    }

    /// <inheritdoc />
    public void ShowDialog<TWindow>(Window owner) where TWindow : Window
    {
        var window = _serviceProvider.GetRequiredService<TWindow>();
        window.ShowDialog(owner);
    }
    
    /// <inheritdoc />
    public void CloseWindow(Window window)
    {
        window.Close();
    }
    
    /// <inheritdoc />
    public void CloseCurrentWindow()
    {
        _currentWindow?.Close();
        _currentWindow = null;
    }
    
    /// <inheritdoc />
    public void SetCurrentWindow(Window window)
    {
        _currentWindow = window;
    }
    
    public Window? GetMainWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            return desktopLifetime.MainWindow ?? desktopLifetime.Windows.FirstOrDefault();
        }

        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
        {
            return singleViewLifetime.MainView as Window;
        }

        return TopLevel.GetTopLevel(null) as Window;
    }

    public Window? GetActiveWindow()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            return desktopLifetime.Windows.FirstOrDefault(w => w.IsActive) 
                   ?? desktopLifetime.MainWindow 
                   ?? desktopLifetime.Windows.FirstOrDefault();
        }

        return GetMainWindow();
    }

    public void Logout()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            // Скрываем все окна кроме авторизации
            foreach (var window in desktopLifetime.Windows.ToList())
            {
                if (window is MainWindow mainWindow)
                {
                    // Скрываем вместо закрытия
                    mainWindow.Hide();
                }
                else if (window is not AuthorizationWindow)
                {
                    window.Close();
                }
            }
        
            // Показываем окно авторизации
            var authWindow = _serviceProvider.GetRequiredService<AuthorizationWindow>();
            authWindow.Show();
        
            // Устанавливаем как главное окно
            desktopLifetime.MainWindow = authWindow;
        
            _currentWindow = authWindow;
        }
    }
}