using Avalonia;
using Avalonia.ReactiveUI;
using System;
using RealtorTool.Data;
using RealtorTool.Desktop.ViewModels;
using RealtorTool.Desktop.Views;
using RealtorTool.Desktop.Views.Windows;
using RealtorTool.Services.Implementations;
using RealtorTool.Services.Interfaces;
using MainWindow = RealtorTool.Desktop.Views.Windows.MainWindow;

namespace RealtorTool.Desktop;

using Avalonia;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

sealed class Program
{
    public static IServiceProvider ServiceProvider { get; private set; }

    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp()
    {
        var services = new ServiceCollection();
        ConfigureServices(services);
        ServiceProvider = services.BuildServiceProvider();

        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace()
            .UseReactiveUI();
    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services.AddDbContext<DataContext>(options => 
            options.UseNpgsql("Server=localhost;port=5415;user id=postgres;password=toor;database=Realtor;"));
        
        services.AddSingleton<IWindowService, WindowService>();

        services.AddTransient<MainWindow>();
        services.AddTransient<AuthorizationWindow>();
        
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<AuthorizationWindowViewModel>();
    }
}