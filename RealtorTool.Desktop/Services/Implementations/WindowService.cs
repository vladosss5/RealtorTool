using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Desktop.Services.Interfaces;
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
        // Для Classic Desktop приложений
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            return desktopLifetime.MainWindow ?? desktopLifetime.Windows.FirstOrDefault();
        }

        // Для Single View приложений
        if (Application.Current?.ApplicationLifetime is ISingleViewApplicationLifetime singleViewLifetime)
        {
            return singleViewLifetime.MainView as Window;
        }

        // Универсальный способ через TopLevel
        return TopLevel.GetTopLevel(null) as Window;
    }

    public Window? GetActiveWindow()
    {
        // Для Classic Desktop приложений
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            return desktopLifetime.Windows.FirstOrDefault(w => w.IsActive) 
                   ?? desktopLifetime.MainWindow 
                   ?? desktopLifetime.Windows.FirstOrDefault();
        }

        // Для других типов приложений
        return GetMainWindow();
    }
}