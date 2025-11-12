using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.ViewModels.Items;
using RealtorTool.Desktop.ViewModels.Windows;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class ApplicationsPageViewModel : PageViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;

    private ListingItemViewModel? _selectedListing;

    [Reactive] public ObservableCollection<ListingItemViewModel> Listings { get; set; } = new();

    public ListingItemViewModel? SelectedListing
    {
        get => _selectedListing;
        set
        {
            if (string.IsNullOrEmpty(value?.Listing.Id))
                MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Id заявки не найден")
                    .ShowAsync();
            
            this.RaiseAndSetIfChanged(ref _selectedListing, value);
            
            OpenListingDetailAsync(value?.Listing.Id!);
        }
    }

    public ApplicationsPageViewModel(
        IServiceProvider serviceProvider, 
        INavigationService navigationService)
    {
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;

        _ = LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var dataContext = scope.ServiceProvider.GetRequiredService<DataContext>();
            var listings = await dataContext.Listings
                .Include(l => l.Realty)
                .ThenInclude(r => r.Photos.OrderBy(p => p.SortOrder).Take(1))
                .Include(l => l.Currency)
                .Include(l => l.ListingType)
                .Include(l => l.Status)
                .ToListAsync();

            Listings.Clear();

            foreach (var listing in listings)
            {
                Listings.Add(new ListingItemViewModel(listing));
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", (ex.InnerException?.Message != null ? ex.InnerException?.Message! : ex.Message))
                .ShowAsync();
        }
    }

    private void OpenListingDetailAsync(string listingId)
    {
        var detailVm = _serviceProvider.GetRequiredService<ListingDetailViewModel>();

        if (detailVm is IParameterReceiver parameterReceiver)
            parameterReceiver.ReceiveParameter(listingId);

        // Отправляем через MessageBus с правильным токеном
        MessageBus.Current.SendMessage(detailVm, "NavigateToPage");

        // Используем сервис навигации
        _navigationService.NavigateTo(detailVm);
        
        MessageBus.Current.SendMessage(listingId, "ListingDetailsId");
    }
}