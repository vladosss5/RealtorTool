using Avalonia;
using Avalonia.ReactiveUI;
using System;
using RealtorTool.Data;
using RealtorTool.Desktop.ViewModels;
using RealtorTool.Desktop.ViewModels.Pages;
using RealtorTool.Desktop.Views.Pages;
using RealtorTool.Desktop.Views.Windows;
using RealtorTool.Services.Implementations;
using RealtorTool.Services.Interfaces;
using MainWindow = RealtorTool.Desktop.Views.Windows.MainWindow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Desktop.ViewModels.Windows;

namespace RealtorTool.Desktop;

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
        // Регистрация сервисов
        services.AddDbContext<DataContext>(options => 
            options.UseNpgsql("Server=localhost;port=5415;user id=postgres;password=toor;database=Realtor;"));
        
        services.AddSingleton<IWindowService, WindowService>();
    
        // Регистрация ViewModels
        services.AddTransient<AuthorizationWindowViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<MyProfilePageViewModel>();
        services.AddTransient<PersonProfilePageViewModel>();
        services.AddTransient<HomePageViewModel>();
        services.AddTransient<CreatingApplicationPageViewModel>();
    
        // Регистрация окон
        services.AddTransient<AuthorizationWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<MyProfilePageView>();
        services.AddTransient<PersonProfilePageView>();
        services.AddTransient<HomePageView>();
        services.AddTransient<CreatingApplicationPageView>();
    }
}