using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.DbEntities.RealtyModels;
using RealtorTool.Core.DbEntities.Views;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class ListingDetailViewModel : PageViewModelBase, IParameterReceiver
{
    private readonly DataContext _context;
    private readonly IMatchingService _matchingService;
    private Employee _authEmployee;
    
    [Reactive] public Listing? Listing { get; set; }
    [Reactive] public PageViewModelBase RealtyViewModel { get; set; }
    [Reactive] public ClientRequest? SellRequest { get; set; }
    [Reactive] public ObservableCollection<PotentialMatch> FoundMatches { get; set; } = new();
    [Reactive] public bool HasMatches { get; set; }
    [Reactive] public bool IsSearching { get; set; }
    [Reactive] public PotentialMatch SelectedMatch { get; set; }
    [Reactive] public bool IsMatchSelected { get; set; }
    [Reactive] public bool IsCreatingDeal { get; set; }
    
    public ReactiveCommand<Unit, Unit> SearchMatchesCommand { get; }
    public ReactiveCommand<PotentialMatch, Unit> SelectMatchCommand { get; }
    public ReactiveCommand<Unit, Unit> RefreshListingCommand { get; }
    
    public string OwnerFullName => Listing?.Owner != null 
        ? $"{Listing.Owner.FirstName} {Listing.Owner.LastName}".Trim() 
        : string.Empty;

    public string EmployeeFullName => Listing?.ResponsibleEmployee != null 
        ? $"{Listing.ResponsibleEmployee.FirstName} {Listing.ResponsibleEmployee.LastName}".Trim() 
        : string.Empty;

    public ListingDetailViewModel(
        DataContext context, 
        IMatchingService matchingService)
    {
        _context = context;
        _matchingService = matchingService;
        
        SearchMatchesCommand = ReactiveCommand.CreateFromTask(SearchMatchesAsync);
        SelectMatchCommand = ReactiveCommand.Create<PotentialMatch>(SelectMatch);
        RefreshListingCommand = ReactiveCommand.CreateFromTask(RefreshListingAsync);
        
        GetDataFromMessageBus();
    }

    private void GetDataFromMessageBus()
    {
        
        MessageBus.Current
            .Listen<string>("ListingDetailsId")
            .SelectMany(async listingId =>
            {
                await LoadListingAsync(listingId);
                await LoadSellRequestAsync();
                Title = $"Заявка №{listingId}";
                LoadRealtyView();
                await SearchMatchesAsync();
                return Unit.Default;
            })
            .Subscribe();
        
        MessageBus.Current
            .Listen<Employee>("CurrentAuth")
            .Subscribe(x => 
            {
                _authEmployee = x;
            });
    }

    public void ReceiveParameter(object parameter)
    {
        if (parameter is string listingId)
        {
            _ = LoadListingAsync(listingId);
        }
    }

    private async Task LoadListingAsync(string listingId)
    {
        try
        {
            Listing = await _context.Listings
                .Include(l => l.Realty)
                .Include(l => l.ListingType)
                .Include(l => l.Status)
                .Include(l => l.Currency)
                .Include(l => l.Owner)
                .Include(l => l.ResponsibleEmployee)
                .Include(l => l.ClientRequests)
                .FirstOrDefaultAsync(l => l.Id == listingId);

            if (Listing != null)
            {
                Title = $"Заявка №{Listing.Id} - {Listing.ListingType?.Value}";
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Не удалось загрузить заявку: {ex.Message}")
                .ShowAsync();
        }
    }

    private async Task LoadSellRequestAsync()
    {
        if (Listing?.Id == null) return;

        try
        {
            // Ищем заявку на продажу/сдачу для этого листинга
            SellRequest = await _context.ClientRequests
                .Include(cr => cr.Client)
                .Include(cr => cr.Employee)
                .FirstOrDefaultAsync(cr => 
                    cr.ListingId == Listing.Id && 
                    (cr.Type == ApplicationType.Sale || cr.Type == ApplicationType.RentOut));

            if (SellRequest == null)
            {
                // Можно создать заявку автоматически или показать сообщение
                await MessageBoxManager
                    .GetMessageBoxStandard("Информация", 
                        "Для этого объявления не найдена заявка на продажу/сдачу. " +
                        "Создайте заявку для поиска совпадений.")
                    .ShowAsync();
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Ошибка при загрузке заявки: {ex.Message}")
                .ShowAsync();
        }
    }
    
    private void LoadRealtyView()
    {
        var realty = Listing?.Realty;
        
        if (realty == null)
        {
            RealtyViewModel = null;
            return;
        }
        
        switch (realty)
        {
            case Apartment apt:
                RealtyViewModel = new ApartmentDetailPageViewModel();
                MessageBus.Current.SendMessage(apt);
                break;

            case PrivateHouse house:
                RealtyViewModel = new PrivateHouseDetailPageViewModel();
                MessageBus.Current.SendMessage(house);
                break;

            case Area area:
                RealtyViewModel = new AreaDetailPageViewModel();
                MessageBus.Current.SendMessage(area);
                break;

            default:
                RealtyViewModel = null;
                break;
        }
    }

    private async Task SearchMatchesAsync()
    {
        if (SellRequest?.Id == null) 
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Информация", 
                    "Не найдена заявка на продажу/сдачу для поиска совпадений")
                .ShowAsync();
            return;
        }

        IsSearching = true;
        
        try
        {
            var matches = await _matchingService.FindPotentialMatchesForSellRequestAsync(SellRequest.Id);
            FoundMatches.Clear();
            FoundMatches.AddRange(matches);
            HasMatches = matches.Any();
            
            // Сбрасываем выбранный матч
            SelectedMatch = null;
            IsMatchSelected = false;

            if (!HasMatches)
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Информация", "Подходящих совпадений не найдено")
                    .ShowAsync();
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Ошибка при поиске совпадений: {ex.Message}")
                .ShowAsync();
        }
        finally
        {
            IsSearching = false;
        }
    }

    private void SelectMatch(PotentialMatch match)
    {
        SelectedMatch = match;
        IsMatchSelected = match != null;
    }

    public async Task CreateDealAsync(PotentialMatch match)
    {
        if (IsCreatingDeal || match == null || SellRequest?.Id == null) return;

        IsCreatingDeal = true;
        
        try
        {
            // Подтверждение создания сделки
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                "Создание сделки",
                $"Вы уверены, что хотите создать сделку?\n\n" +
                $"Объект: {match.RealtyName}\n" +
                $"Тип: {match.RealtyTypeDisplayName}\n" +
                $"Район: {match.District}\n" +
                $"Цена: {match.ListingPrice:N0} руб.\n" +
                $"Качество совпадения: {match.MatchScore}%",
                ButtonEnum.YesNo,
                Icon.Question
            );

            var result = await messageBox.ShowAsync();
        
            if (result == ButtonResult.Yes)
            {
                // Создаем сделку через сервис matching
                var createResult = await _matchingService.CreateMatchAsync(match.BuyRequestId, match.SellRequestId);
            
                if (createResult)
                {
                    await MessageBoxManager
                        .GetMessageBoxStandard("Успех", "Сделка успешно создана!")
                        .ShowAsync();
                
                    // Обновляем статусы и перезагружаем данные
                    await UpdateRequestStatuses();
                    await RefreshListingAsync();
                    await SearchMatchesAsync();
                    
                    // Отправляем сообщение об обновлении
                    MessageBus.Current.SendMessage("DealCreated", "GlobalNotification");
                }
                else
                {
                    await MessageBoxManager
                        .GetMessageBoxStandard("Ошибка", "Не удалось создать сделку")
                        .ShowAsync();
                }
            }
        }
        catch (Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Ошибка при создании сделки: {ex.Message}")
                .ShowAsync();
        }
        finally
        {
            IsCreatingDeal = false;
        }
    }

    private async Task UpdateRequestStatuses()
    {
        if (SellRequest?.Id == null) return;

        try
        {
            // Обновляем статус заявки на продажу
            var matchedStatus = ApplicationStatus.Matched;
            SellRequest.Status = matchedStatus;
            
            // Сохраняем изменения
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при обновлении статусов: {ex.Message}");
        }
    }

    private async Task RefreshListingAsync()
    {
        if (Listing?.Id != null)
        {
            await LoadListingAsync(Listing.Id);
            await LoadSellRequestAsync();
        }
    }
    
    // Свойства для отображения статистики
    public string MatchesSummary => HasMatches 
        ? $"Найдено {FoundMatches.Count} совпадений" 
        : "Совпадения не найдены";
        
    public int GoodMatchesCount => FoundMatches.Count(m => m.IsGoodMatch);
    public int PerfectMatchesCount => FoundMatches.Count(m => m.IsPerfectMatch);
    
    public string MatchQualityDescription
    {
        get
        {
            if (!HasMatches) return "Совпадения не найдены";
            if (PerfectMatchesCount > 0) return $"{PerfectMatchesCount} идеальных совпадений";
            if (GoodMatchesCount > 0) return $"{GoodMatchesCount} хороших совпадений";
            return "Совпадения низкого качества";
        }
    }

    public string ListingStatusDescription => Listing?.Status?.Value ?? "Неизвестно";
    public bool CanCreateDeals => HasMatches && !IsCreatingDeal;
}