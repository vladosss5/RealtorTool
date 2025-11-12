using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using HotAvalonia;
using RealtorTool.Desktop.ViewModels;
using RealtorTool.Desktop.ViewModels.Pages;
using RealtorTool.Desktop.Views.Pages;
using RealtorTool.Desktop.Views.Windows;
using MainWindow = RealtorTool.Desktop.Views.Windows.MainWindow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Converters;
using RealtorTool.Desktop.Extentions;
using RealtorTool.Desktop.Services.Implementations;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.ViewModels.Items;
using RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;
using RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;
using RealtorTool.Desktop.ViewModels.Windows;
using RealtorTool.Desktop.Views.Pages.ApplicationPages;
using RealtorTool.Desktop.Views.Pages.RealtyDetailPages;

namespace RealtorTool.Desktop;

sealed class Program
{
    public static IServiceProvider? ServiceProvider { get; private set; }

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
        services.AddDataBase();
        
        services.AddConvertors();
        
        services.AddServices();
    
        services.AddViewModels();
        
        services.AddViews();
    }
}