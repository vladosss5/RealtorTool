using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class DealDetailPageViewModel : PageViewModelBase, IParameterReceiver
{
    private readonly DataContext _dataContext;
    private readonly INavigationService _navigationService;

    [Reactive] public Deal? Deal { get; set; }
    [Reactive] public ObservableCollection<DictionaryValue> AvailableStatuses { get; set; } = new();
    [Reactive] public DictionaryValue? SelectedStatus { get; set; }
    [Reactive] public bool IsLoading { get; set; }
    [Reactive] public bool IsEditing { get; set; }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand EditCommand { get; }

    public DealDetailPageViewModel(
        DataContext dataContext,
        INavigationService navigationService)
    {
        _dataContext = dataContext;
        _navigationService = navigationService;

        SaveCommand = ReactiveCommand.CreateFromTask(SaveDealAsync);
        CancelCommand = ReactiveCommand.Create(CancelEdit);
        EditCommand = ReactiveCommand.Create(StartEdit);
    }

    public void ReceiveParameter(object parameter)
    {
        if (parameter is string dealId)
        {
            _ = LoadDealAsync(dealId);
        }
    }

    private async Task LoadDealAsync(string dealId)
    {
        IsLoading = true;

        try
        {
            // Загружаем сделку со всеми связанными данными
            Deal = await _dataContext.Deals
                .Include(d => d.Listing)
                    .ThenInclude(l => l.Realty)
                    .ThenInclude(r => r.Address)
                .Include(d => d.Listing)
                    .ThenInclude(l => l.Currency)
                .Include(d => d.Listing)
                    .ThenInclude(l => l.Realty)
                    .ThenInclude(r => r.Photos.OrderBy(p => p.SortOrder))
                .Include(d => d.Buyer)
                .Include(d => d.Employee)
                .Include(d => d.DealType)
                .Include(d => d.Status)
                .Include(d => d.Participants)
                    .ThenInclude(p => p.ClientRequest)
                    .ThenInclude(cr => cr.Client)
                .FirstOrDefaultAsync(d => d.Id == dealId && !d.IsDeleted);

            if (Deal == null)
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Сделка не найдена")
                    .ShowAsync();
                _navigationService.GoBack();
                return;
            }
            

            // Загружаем доступные статусы для сделок
            await LoadAvailableStatusesAsync();

            // Устанавливаем текущий статус
            SelectedStatus = AvailableStatuses.FirstOrDefault(s => s.Id == Deal.StatusId);
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Ошибка загрузки сделки: {ex.Message}")
                .ShowAsync();
        }
        finally
        {
            IsLoading = false;
        }
    }

    private async Task LoadAvailableStatusesAsync()
    {
        try
        {
            var statuses = await _dataContext.DictionaryValues
                .Where(dv => dv.DictionaryId == "deal_status")
                .OrderBy(dv => dv.Value)
                .ToListAsync();

            AvailableStatuses.Clear();
            AvailableStatuses.AddRange(statuses);
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Ошибка загрузки статусов: {ex.Message}")
                .ShowAsync();
        }
    }

    private void StartEdit()
    {
        IsEditing = true;
    }

    private void CancelEdit()
    {
        IsEditing = false;
        // Восстанавливаем исходный статус
        SelectedStatus = AvailableStatuses.FirstOrDefault(s => s.Id == Deal?.StatusId);
    }

    private async Task SaveDealAsync()
    {
        if (Deal == null || SelectedStatus == null)
            return;

        try
        {
            // Обновляем статус сделки
            Deal.StatusId = SelectedStatus.Id;
            Deal.Status = SelectedStatus;

            _dataContext.Deals.Update(Deal);
            await _dataContext.SaveChangesAsync();

            IsEditing = false;

            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Сделка успешно обновлена", ButtonEnum.Ok, Icon.Success)
                .ShowAsync();
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Ошибка сохранения: {ex.Message}")
                .ShowAsync();
        }
    }

    // Вычисляемые свойства для удобства привязки
    public string BuyerName => Deal?.Buyer != null 
        ? $"{Deal.Buyer.LastName} {Deal.Buyer.FirstName} {Deal.Buyer.MiddleName}"
        : "Не указан";

    public string EmployeeName => Deal?.Employee != null
        ? $"{Deal.Employee.LastName} {Deal.Employee.FirstName} {Deal.Employee.MiddleName}"
        : "Не назначен";

    public string RealtyName => Deal?.Listing?.Realty?.Name ?? "Не указан";

    public string Address => Deal?.Listing?.Realty?.Address != null
        ? $"{Deal.Listing.Realty.Address.City}, {Deal.Listing.Realty.Address.Street} {Deal.Listing.Realty.Address.HouseNumber}"
        : "Адрес не указан";

    public string FormattedFinalPrice => Deal != null ? $"{Deal.FinalPrice:N0} {Deal.Listing?.Currency?.Value}" : "";
    
    public string FormattedCommission => Deal != null ? $"{Deal.Commission:N0} {Deal.Listing?.Currency?.Value}" : "";

    public bool HasPhotos => Deal?.Listing?.Realty?.Photos?.Any() == true;

    public ObservableCollection<Photo> Photos => new(Deal?.Listing?.Realty?.Photos?.OrderBy(p => p.SortOrder) ?? Enumerable.Empty<Photo>());
}