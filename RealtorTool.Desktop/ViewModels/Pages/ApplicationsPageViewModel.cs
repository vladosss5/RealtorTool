using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Services.Implementations;
using RealtorTool.Desktop.ViewModels.Windows;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class ApplicationsPageViewModel : PageViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly DataContext _dataContext;

    [Reactive] public ObservableCollection<Listing> Listings { get; set; } = new();
    [Reactive] public Listing? SelectedListing { get; set; }

    public ApplicationsPageViewModel(
        DataContext dataContext, 
        IServiceProvider serviceProvider)
    {
        _dataContext = dataContext;
        _serviceProvider = serviceProvider;
        
        this.WhenAnyValue(x => x.SelectedListing)
            .Where(listing => listing != null)
            .Subscribe(async listing => await OpenListingDetailAsync(listing!));
        
        _ = LoadDataAsync();
    }

    public async Task LoadDataAsync()
    {
        var listings = await _dataContext.Listings
            .Include(l => l.Realty)
            .Include(l => l.Currency)
            .Include(l => l.ListingType)
            .Include(l => l.Status)
            .ToListAsync();
        Listings.AddRange(listings);
    }
    
    private async Task OpenListingDetailAsync(Listing listing)
    {
        if (listing != null)
        {
            MessageBus.Current.SendMessage(listing);
            
            var detailVm = _serviceProvider.GetRequiredService<ListingDetailViewModel>();
            if (detailVm is IParameterReceiver parameterReceiver)
            {
                parameterReceiver.ReceiveParameter(listing.Id);
            }
    
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
            {
                var mainWindow = desktopLifetime.MainWindow;
                if (mainWindow?.DataContext is MainWindowViewModel mainVm)
                {
                    mainVm.SelectedPageItem = detailVm;
                }
            }
        }
    }
}