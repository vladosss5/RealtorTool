using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.DbEntities.Views;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

public class BuyApplicationPageViewModel : PageViewModelBase
{
    private readonly DataContext _context;
    private readonly IMatchingService _matchingService;

    public Employee _authEmploee { get; private set; }

    [Reactive] public Client NewClient { get; set; } = new();
    [Reactive] public decimal? MaxPrice { get; set; }
    [Reactive] public int? MinRooms { get; set; }
    [Reactive] public decimal? MinArea { get; set; }
    [Reactive] public decimal? MaxArea { get; set; }
    [Reactive] public string DesiredLocation { get; set; }
    [Reactive] public string AdditionalRequirements { get; set; }
    [Reactive] public ObservableCollection<PotentialMatch> FoundMatches { get; set; } = new();
    [Reactive] public bool HasMatches { get; set; }
    [Reactive] public bool IsSearching { get; set; }
    [Reactive] public PotentialMatch SelectedMatch { get; set; }
    
    [Reactive] public bool IsMatchSelected { get; set; }
    [Reactive] public bool IsCreatingRequest { get; set; }
    

    private int _selectedPropertyTypeIndex;
    public int SelectedPropertyTypeIndex
    {
        get => _selectedPropertyTypeIndex;
        set => this.RaiseAndSetIfChanged(ref _selectedPropertyTypeIndex, value);
    }

    public RealtyType CurrentRealtyType => SelectedPropertyTypeIndex switch
    {
        0 => RealtyType.Apartment,
        1 => RealtyType.Area,
        2 => RealtyType.PrivateHouse,
        _ => RealtyType.Apartment
    };

    public ReactiveCommand<Unit, Unit> CreateBuyRequestCommand { get; }
    public ReactiveCommand<Unit, Unit> SearchMatchesCommand { get; }
    public ReactiveCommand<PotentialMatch, Unit> SelectMatchButton { get; }
    public ReactiveCommand<PotentialMatch, Unit> ViewMatchDetailsCommand { get; }

    public BuyApplicationPageViewModel(DataContext context, IMatchingService matchingService)
    {
        _context = context;
        _matchingService = matchingService;
        
        GetDataFromMessageBus();
        
        CreateBuyRequestCommand = ReactiveCommand.CreateFromTask(CreateBuyRequestAsync);
        SearchMatchesCommand = ReactiveCommand.CreateFromTask(SearchMatchesAsync);
        SelectMatchButton = ReactiveCommand.CreateFromTask<PotentialMatch>(SelectMatchAsync);
        // ДОБАВИТЬ: Инициализация команды просмотра деталей
        ViewMatchDetailsCommand = ReactiveCommand.Create<PotentialMatch>(match =>
        {
            SelectedMatch = match;
            IsMatchSelected = true;
        });
    }
    
    private void GetDataFromMessageBus()
    {
        MessageBus.Current
            .Listen<Employee>("CurrentAuth")
            .Subscribe(x => 
            {
                _authEmploee = x;
            });

        if (_authEmploee != null)
            return;

        _authEmploee = _context.Employees.Find("system_empl")!;
    }

    private async Task CreateBuyRequestAsync()
    {
        if (IsCreatingRequest) return;

        IsCreatingRequest = true;
        
        await using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            if (string.IsNullOrWhiteSpace(NewClient.FirstName) || string.IsNullOrWhiteSpace(NewClient.LastName))
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Заполните имя и фамилию клиента")
                    .ShowAsync();
                return;
            }

            if (_authEmploee == null)
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Не удалось определить сотрудника для обработки заявки")
                    .ShowAsync();
                return;
            }

            await _context.Clients.AddAsync(NewClient);
            await _context.SaveChangesAsync();

            var buyRequest = new ClientRequest
            {
                DesiredRealtyType = CurrentRealtyType,
                Type = ApplicationType.Purchase,
                Status = ApplicationStatus.New,
                ClientId = NewClient.Id,
                EmployeeId = _authEmploee.Id,
                MaxPrice = MaxPrice,
                MinRooms = MinRooms,
                MinArea = MinArea,
                MaxArea = MaxArea,
                DesiredLocation = DesiredLocation,
                AdditionalRequirements = AdditionalRequirements,
                CreatedDate = DateTime.UtcNow
            };

            await _context.ClientRequests.AddAsync(buyRequest);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();

            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Заявка на покупку создана")
                .ShowAsync();

            await SearchMatchesAsync();
        }
        catch (DbUpdateException dbEx)
        {
            await transaction.RollbackAsync();
            var errorMessage = $"Ошибка базы данных: {dbEx.InnerException?.Message ?? dbEx.Message}";
            await MessageBoxManager.GetMessageBoxStandard("Ошибка базы данных", errorMessage).ShowAsync();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            var errorMessage = $"Неожиданная ошибка: {ex.Message}";
            await MessageBoxManager.GetMessageBoxStandard("Ошибка", errorMessage).ShowAsync();
        }
        finally
        {
            IsCreatingRequest = false;
        }
    }
    
    private async Task SearchMatchesAsync()
    {
        if (NewClient?.Id == null) 
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Информация", "Сначала создайте заявку клиента")
                .ShowAsync();
            return;
        }

        IsSearching = true;
        
        try
        {
            // Находим созданную заявку
            var buyRequest = await _context.ClientRequests
                .FirstOrDefaultAsync(cr => cr.ClientId == NewClient.Id && cr.Type == ApplicationType.Purchase);

            if (buyRequest != null)
            {
                var matches = await _matchingService.FindPotentialMatchesAsync(buyRequest.Id);
                FoundMatches.Clear();
                FoundMatches.AddRange(matches);
                HasMatches = matches.Any();
                
                // Сбрасываем выбранный матч
                SelectedMatch = null;
                IsMatchSelected = false;
            }
            else
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Информация", "Не найдена заявка для поиска совпадений")
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

    public async Task SelectMatchAsync(PotentialMatch selectedMatch)
    {
        if (NewClient?.Id == null || selectedMatch == null) return;

        var buyRequest = await _context.ClientRequests
            .FirstOrDefaultAsync(cr => cr.ClientId == NewClient.Id && cr.Type == ApplicationType.Purchase);

        if (buyRequest != null)
        {
            var messageBox = MessageBoxManager.GetMessageBoxStandard(
                "Подтверждение",
                $"Вы уверены, что хотите создать сделку для этого объекта?\n\n" +
                $"Цена: {selectedMatch.ListingPrice:N0} руб.\n" +
                $"Тип: {GetRealtyTypeDisplayName(selectedMatch.RealtyType)}\n" +
                $"Район: {selectedMatch.District}",
                ButtonEnum.YesNo,
                Icon.Question
            );

            var result = await messageBox.ShowAsync();
        
            if (result == ButtonResult.Yes)
            {
                var createResult = await _matchingService.CreateMatchAsync(buyRequest.Id, selectedMatch.SellRequestId);
            
                if (createResult)
                {
                    await MessageBoxManager
                        .GetMessageBoxStandard("Успех", "Сделка создана успешно!")
                        .ShowAsync();
                
                    ClearForm();
                }
                else
                {
                    await MessageBoxManager
                        .GetMessageBoxStandard("Ошибка", "Не удалось создать сделку. Попробуйте позже.")
                        .ShowAsync();
                }
            }
        }
    }

    private string GetRealtyTypeDisplayName(RealtyType realtyType)
    {
        return realtyType switch
        {
            RealtyType.Apartment => "Квартира",
            RealtyType.PrivateHouse => "Частный дом",
            RealtyType.Area => "Земельный участок",
            _ => "Неизвестно"
        };
    }

    private void ClearForm()
    {
        NewClient = new Client();
        MaxPrice = null;
        MinRooms = null;
        MinArea = null;
        MaxArea = null;
        DesiredLocation = string.Empty;
        AdditionalRequirements = string.Empty;
        FoundMatches.Clear();
        HasMatches = false;
        SelectedMatch = null;
        IsMatchSelected = false;
    }
    
    public string MatchesSummary => HasMatches 
        ? $"Найдено {FoundMatches.Count} совпадений" 
        : "Совпадения не найдены";
        
    public int GoodMatchesCount => FoundMatches.Count(m => m.IsGoodMatch);
    public int PerfectMatchesCount => FoundMatches.Count(m => m.IsPerfectMatch);
}