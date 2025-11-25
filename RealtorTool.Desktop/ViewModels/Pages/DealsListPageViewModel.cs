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

namespace RealtorTool.Desktop.ViewModels.Pages;

public class DealsListPageViewModel : PageViewModelBase
{
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;

    private DealItemViewModel? _selectedDeal;

    [Reactive] public ObservableCollection<DealItemViewModel> Deals { get; set; } = new();

    public DealItemViewModel? SelectedDeal
    {
        get => _selectedDeal;
        set
        {
            if (string.IsNullOrEmpty(value?.Deal.Id))
            {
                MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Id сделки не найден")
                    .ShowAsync();   
            }
            
            OpenDealDetailAsync(value?.Deal.Id!);

            _selectedDeal = null;
        }
    }

    public DealsListPageViewModel(
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
            
            var deals = await dataContext.Deals
                .Include(d => d.Listing)
                .ThenInclude(l => l.Realty)
                .ThenInclude(r => r.Photos.OrderBy(p => p.SortOrder).Take(1))
                .Include(d => d.Listing)
                .ThenInclude(l => l.Currency)
                .Include(d => d.Buyer)
                .Include(d => d.Employee)
                .Include(d => d.DealType)
                .Include(d => d.Status)
                .Include(d => d.Participants)
                .ThenInclude(p => p.ClientRequest)
                .ThenInclude(cr => cr.Client)
                .Where(d => !d.IsDeleted)
                .ToListAsync();

            Deals.Clear();

            foreach (var deal in deals)
            {
                Deals.Add(new DealItemViewModel(deal));
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", (ex.InnerException?.Message != null ? ex.InnerException?.Message! : ex.Message))
                .ShowAsync();
        }
    }

    private void OpenDealDetailAsync(string dealId)
    {
        var detailVm = _serviceProvider.GetRequiredService<DealDetailPageViewModel>();

        if (detailVm is IParameterReceiver parameterReceiver)
            parameterReceiver.ReceiveParameter(dealId);

        MessageBus.Current.SendMessage(detailVm, "NavigateToPage");
        _navigationService.NavigateTo(detailVm);
        MessageBus.Current.SendMessage(dealId, "DealDetailsId");
    }

    // Команда для обновления списка сделок
    public async Task RefreshDealsAsync()
    {
        await LoadDataAsync();
    }
}