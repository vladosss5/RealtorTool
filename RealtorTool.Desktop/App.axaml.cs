using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Desktop.Views.Windows;

namespace RealtorTool.Desktop;

public partial class App : Application
{
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var firstWindow = Program.ServiceProvider.GetRequiredService<AuthorizationWindow>();
            desktop.MainWindow = firstWindow;
        }
        base.OnFrameworkInitializationCompleted();
    }
}