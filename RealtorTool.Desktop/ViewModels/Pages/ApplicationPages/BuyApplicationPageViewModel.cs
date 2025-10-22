using System;
using System.Reactive;
using System.Threading.Tasks;
using MsBox.Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;

namespace RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

public class BuyApplicationPageViewModel : PageViewModelBase
{
    private readonly DataContext _context;

    [Reactive] public Client NewClient { get; set; } = new();
    [Reactive] public decimal? MaxPrice { get; set; }
    [Reactive] public int? MinRooms { get; set; }
    [Reactive] public decimal? MinArea { get; set; }
    [Reactive] public decimal? MaxArea { get; set; }
    [Reactive] public string DesiredLocation { get; set; }
    [Reactive] public string AdditionalRequirements { get; set; }

    // Для выбора типа недвижимости
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

    public BuyApplicationPageViewModel(DataContext context)
    {
        _context = context;
        CreateBuyRequestCommand = ReactiveCommand.CreateFromTask(CreateBuyRequestAsync);
    }

    private async Task CreateBuyRequestAsync()
    {
        try
        {
            // Сохраняем клиента
            await _context.Clients.AddAsync(NewClient);
            await _context.SaveChangesAsync();

            // Создаем заявку на покупку
            var buyRequest = new ClientRequest
            {
                Type = ApplicationType.Purchase,
                Status = ApplicationStatus.New,
                ClientId = NewClient.Id,
                EmployeeId = "temp_employee_id", // TODO: Заменить на ID текущего сотрудника
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

            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Заявка на покупку создана")
                .ShowAsync();
        }
        catch (Exception e)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", e.Message)
                .ShowAsync();
        }
    }
}