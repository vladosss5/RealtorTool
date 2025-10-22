using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Desktop.Views.Windows;
using HotAvalonia;

namespace RealtorTool.Desktop;

public partial class App : Application
{
    public override void Initialize()
    {
        this.EnableHotReload();
        
        base.Initialize();
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var firstWindow = Program.ServiceProvider.GetRequiredService<MainWindow>();
            desktop.MainWindow = firstWindow;
        }
        base.OnFrameworkInitializationCompleted();
    }
}