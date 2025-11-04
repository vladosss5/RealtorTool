using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using HotAvalonia;
using RealtorTool.Desktop.ViewModels;
using RealtorTool.Desktop.ViewModels.Pages;
using RealtorTool.Desktop.Views.Pages;
using RealtorTool.Desktop.Views.Windows;
using RealtorTool.Services.Interfaces;
using MainWindow = RealtorTool.Desktop.Views.Windows.MainWindow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Converters;
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
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
    
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    
        // Регистрация DataContext
        services.AddDbContext<DataContext>(options => 
            options.UseNpgsql(connectionString));
        
        // Регистрация сервисов
        services.AddSingleton<DateTimeFormatConverter>();
        services.AddSingleton<StatusToColorConverter>();
        services.AddSingleton<MatchScoreToColorConverter>();
        services.AddSingleton<MatchScoreToBackgroundConverter>();
        services.AddSingleton<ByteArrayToImageConverter>();
        services.AddSingleton<BooleanToColorConverter>();
        services.AddSingleton<MatchesCountToColorConverter>();
        services.AddSingleton<TrueToFalseConverter>();
        services.AddSingleton<ObjectIsNotNullConverter>();
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<IAccountingService, AccountingService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IPhotoService, PhotoService>();
        services.AddSingleton<IMatchingService, MatchingService>();
    
        // Регистрация ViewModels
        services.AddTransient<AuthorizationWindowViewModel>();
        services.AddTransient<MainWindowViewModel>();
        services.AddTransient<MyProfilePageViewModel>();
        services.AddTransient<PersonProfilePageViewModel>();
        services.AddTransient<HomePageViewModel>();
        services.AddTransient<CreatingApplicationPageViewModel>();
        services.AddTransient<EmployeesPageViewModel>();
        services.AddTransient<ApplicationsPageViewModel>();
        services.AddTransient<ListingDetailViewModel>();
        services.AddTransient<ApartmentDetailPageViewModel>();
        services.AddTransient<AreaDetailPageViewModel>();
        services.AddTransient<PrivateHouseDetailPageViewModel>();
        services.AddTransient<ListingItemViewModel>();
        
        // Регистрация страниц с ленивой загрузкой
        services.AddTransient<BuyApplicationPageViewModel>();
        services.AddTransient<Func<BuyApplicationPageViewModel>>(sp => 
            () => sp.GetRequiredService<BuyApplicationPageViewModel>());
        
        services.AddTransient<SellApplicationPageViewModel>();
        services.AddTransient<Func<SellApplicationPageViewModel>>(sp => 
            () => sp.GetRequiredService<SellApplicationPageViewModel>());
        
        services.AddTransient<LeaseApplicationPageViewModel>();
        services.AddTransient<Func<LeaseApplicationPageViewModel>>(sp => 
            () => sp.GetRequiredService<LeaseApplicationPageViewModel>());
        
        services.AddTransient<RentalApplicationPageViewModel>();
        services.AddTransient<Func<RentalApplicationPageViewModel>>(sp => 
            () => sp.GetRequiredService<RentalApplicationPageViewModel>());
    
        // Регистрация View
        services.AddTransient<AuthorizationWindow>();
        services.AddTransient<MainWindow>();
        services.AddTransient<MyProfilePageView>();
        services.AddTransient<PersonProfilePageView>();
        services.AddTransient<HomePageView>();
        services.AddTransient<CreatingApplicationPageView>();
        services.AddTransient<EmployeesPageView>();
        services.AddTransient<BuyApplicationPageView>();
        services.AddTransient<SellApplicationPageView>();
        services.AddTransient<LeaseApplicationPageView>();
        services.AddTransient<RentalApplicationPageView>();
        services.AddTransient<ApplicationsPageView>();
        services.AddTransient<ListingDetailView>();
        services.AddTransient<ApartmentDetailPageView>();
        services.AddTransient<AreaDetailPageView>();
        services.AddTransient<PrivateHouseDetailPageView>();
    }
}