using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Converters;
using RealtorTool.Desktop.Services.Implementations;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.ViewModels;
using RealtorTool.Desktop.ViewModels.Items;
using RealtorTool.Desktop.ViewModels.Pages;
using RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;
using RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;
using RealtorTool.Desktop.ViewModels.Windows;
using RealtorTool.Desktop.Views.Pages;
using RealtorTool.Desktop.Views.Pages.ApplicationPages;
using RealtorTool.Desktop.Views.Pages.RealtyDetailPages;
using RealtorTool.Desktop.Views.Windows;

namespace RealtorTool.Desktop.Extentions;

public static class ServiceRegistrationExtensions
{
    public static void AddDataBase(this IServiceCollection services)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        
        var connectionString = configuration.GetConnectionString("DefaultConnection");
    
        if (string.IsNullOrEmpty(connectionString))
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    
        services.AddDbContext<DataContext>(options => 
            options.UseNpgsql(connectionString));
    }
    
    public static void AddConvertors(this IServiceCollection services)
    {
        services.AddSingleton<DateTimeFormatConverter>();
        services.AddSingleton<StatusToColorConverter>();
        services.AddSingleton<MatchScoreToColorConverter>();
        services.AddSingleton<MatchScoreToBackgroundConverter>();
        services.AddSingleton<ByteArrayToImageConverter>();
        services.AddSingleton<BooleanToColorConverter>();
        services.AddSingleton<MatchesCountToColorConverter>();
        services.AddSingleton<TrueToFalseConverter>();
        services.AddSingleton<ObjectIsNotNullConverter>();
        services.AddSingleton<AnyRoleToVisibilityConverter>();
        services.AddSingleton<RoleToVisibilityConverter>();
    }
    
    public static void AddServices(this IServiceCollection services)
    {
        services.AddSingleton<IWindowService, WindowService>();
        services.AddSingleton<IAccountingService, AccountingService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IPhotoService, PhotoService>();
        services.AddSingleton<IMatchingService, MatchingService>();
    }
    
    public static void AddViewModels(this IServiceCollection services)
    {
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
        services.AddTransient<EmployeeDetailViewModel>();
        services.AddTransient<DealDetailPageViewModel>();
        
        // Регистрация страниц с ленивой загрузкой
        services.AddTransient<BuyApplicationPageViewModel>();
        services.AddTransient<Func<BuyApplicationPageViewModel>>(sp => 
            () => sp.GetRequiredService<BuyApplicationPageViewModel>());
        
        services.AddTransient<DealsListPageViewModel>();
        services.AddTransient<Func<DealsListPageViewModel>>(sp => 
            () => sp.GetRequiredService<DealsListPageViewModel>());
        
        services.AddTransient<SellApplicationPageViewModel>();
        services.AddTransient<Func<SellApplicationPageViewModel>>(sp => 
            () => sp.GetRequiredService<SellApplicationPageViewModel>());
        
        services.AddTransient<LeaseApplicationPageViewModel>();
        services.AddTransient<Func<LeaseApplicationPageViewModel>>(sp => 
            () => sp.GetRequiredService<LeaseApplicationPageViewModel>());
        
        services.AddTransient<RentalApplicationPageViewModel>();
        services.AddTransient<Func<RentalApplicationPageViewModel>>(sp => 
            () => sp.GetRequiredService<RentalApplicationPageViewModel>());
    }
    
    public static void AddViews(this IServiceCollection services)
    {
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
        services.AddTransient<EmployeeDetailView>();
        services.AddTransient<DealsListPageView>();
        services.AddTransient<DealDetailPageView>();
    }

    // Метод для получения сервисов по условию
    public static T? GetServiceIfAuthorized<T>(this IServiceProvider provider, UserRole requiredRole)
    {
        var authService = provider.GetRequiredService<IAccountingService>();
        return authService.HasRole(requiredRole) 
            ? provider.GetService<T>() 
            : default;
    }
}