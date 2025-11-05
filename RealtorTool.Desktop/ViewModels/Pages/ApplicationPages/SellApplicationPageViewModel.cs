using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.DbEntities.RealtyModels;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.DTOs;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

public class SellApplicationPageViewModel : ViewModelBase
{
    /// Сервисы.
    private readonly DataContext _context;
    private readonly IPhotoService _photoService;

    // Поля.
    [Reactive] public Address NewAddress { get; set; } = new();
    [Reactive] public Listing NewListing { get; set; } = new();
    [Reactive] public Client NewClient { get; set; } = new();
    [Reactive] public Area NewArea { get; set; } = new();
    [Reactive] public PrivateHouse NewPrivateHouse { get; set; } = new();
    [Reactive] public Apartment NewApartament { get; set; } = new();
    [Reactive] public ObservableCollection<UploadedPhoto> UploadedPhotos { get; set; } = new();
    [Reactive] public string PhotosSummary { get; set; } = "Фотографии не добавлены";

    // Коллекции.
    public ObservableCollection<DictionaryValue> RenovationTypes { get; set; } = new();
    public ObservableCollection<DictionaryValue> BathroomTypes { get; set; } = new();

    public RealtyType CurrentRealtyType { get; private set; }

    private int _selectedPropertyTypeIndex;

    public int SelectedPropertyTypeIndex
    {
        get => _selectedPropertyTypeIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedPropertyTypeIndex, value);
            CurrentRealtyType = SelectedPropertyTypeIndex switch
            {
                0 => RealtyType.Apartment,
                1 => RealtyType.Area,
                2 => RealtyType.PrivateHouse,
                _ => CurrentRealtyType
            };
        }
    }

    // Кнопки.
    public ICommand CreateSellRequest { get; }
    public ReactiveCommand<Unit, Unit> SelectImagesCommand { get; }
    public ReactiveCommand<UploadedPhoto, Unit> RemovePhotoCommand { get; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public SellApplicationPageViewModel(
        DataContext context,
        IPhotoService photoService)
    {
        _context = context;
        _photoService = photoService;
        
        CreateSellRequest = ReactiveCommand.CreateFromTask(CreateSellRequestAsync);
        SelectImagesCommand = ReactiveCommand.CreateFromTask(SelectImagesAsync);
        RemovePhotoCommand = ReactiveCommand.Create<UploadedPhoto>(RemovePhoto);

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        var renovationTypesList = await _context.DictionaryValues
            .Where(x => x.DictionaryId == "renovation_type")
            .ToListAsync();
        RenovationTypes.AddRange(renovationTypesList);

        var bathroomTypes = await _context.DictionaryValues
            .Where(x => x.DictionaryId == "bathroom_type")
            .ToListAsync();
        BathroomTypes.AddRange(bathroomTypes);
    }

    private async Task CreateSellRequestAsync()
    {
        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var employee = await _context.Employees.FirstOrDefaultAsync(); // TODO Подключить MB
            if (employee == null)
            {
                await MessageBoxManager
                    .GetMessageBoxStandard("Ошибка", "Не найден сотрудник для обработки заявки")
                    .ShowAsync();
                return;
            }

            _context.Clients.Attach(NewClient);

            _context.Addresses.Attach(NewAddress);

            Realty createdRealty = null;

            switch (CurrentRealtyType)
            {
                case RealtyType.Area:
                    NewArea.Address = NewAddress;
                    NewArea.RealtyType = RealtyType.Area;
                    _context.Areas.Attach(NewArea);
                    createdRealty = NewArea;
                    break;

                case RealtyType.Apartment:
                    NewApartament.RenovationType = RenovationTypes.FirstOrDefault(x => x.IsSelected);
                    NewApartament.BathroomType = BathroomTypes.FirstOrDefault(x => x.IsSelected);
                    NewApartament.Address = NewAddress;
                    NewApartament.RealtyType = RealtyType.Apartment;
                    _context.Apartments.Attach(NewApartament);
                    createdRealty = NewApartament;
                    break;

                case RealtyType.PrivateHouse:
                    NewPrivateHouse.Address = NewAddress;
                    NewPrivateHouse.RealtyType = RealtyType.PrivateHouse;
                    _context.PrivateHouses.Attach(NewPrivateHouse);
                    createdRealty = NewPrivateHouse;
                    break;
            }

            NewListing.Realty = createdRealty;
            NewListing.Owner = NewClient;
            NewListing.CurrencyId = "currency_rub";
            NewListing.ListingTypeId = "listing_sale";
            NewListing.StatusId = "listing_active";
            _context.Listings.Attach(NewListing);

            var sellRequest = new ClientRequest
            {
                Type = ApplicationType.Sale,
                Status = ApplicationStatus.New,
                ClientId = NewClient.Id,
                ListingId = NewListing.Id,
                EmployeeId = employee.Id,
                CreatedDate = DateTime.UtcNow
            };
            _context.ClientRequests.Attach(sellRequest);

            await _context.SaveChangesAsync();

            if (createdRealty != null && UploadedPhotos.Any())
            {
                await _photoService.SavePhotosToDatabaseAsync(
                    UploadedPhotos.ToList(), 
                    createdRealty.Id, 
                    EntityTypeForPhoto.Realty);
            }

            await transaction.CommitAsync();

            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Заявка на продажу успешно создана")
                .ShowAsync();

            ClearForm();
        }
        catch (Exception e)
        {
            var exceptionMessage = $"{e.Message}\n{(e.InnerException?.Message ?? "")}";
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", exceptionMessage)
                .ShowAsync();
        }
    }

    private async Task AddAddressDataToContextAsync()
    {
        await _context.AddAsync(NewAddress);
    }

    private async Task AddPrivateHouseDataToContextAsync()
    {
        throw new NotImplementedException();
    }

    private async Task<Realty> AddApartmentDataToContextAsync()
    {
        NewApartament.RenovationType = RenovationTypes.FirstOrDefault(x => x.IsSelected);
        NewApartament.BathroomType = BathroomTypes.FirstOrDefault(x => x.IsSelected);
        NewApartament.Address = NewAddress;
        NewApartament.RealtyType = RealtyType.Apartment;
        
        await _context.AddAsync(NewApartament);
        await _context.SaveChangesAsync();
        
        return NewApartament;
    }

    private async Task AddAreaDataToContextAsync()
    {
        throw new NotImplementedException();
    }

    private async Task AddClientDataToContextAsync()
    {
        await _context.AddAsync(NewClient);
    }

    private async Task SelectImagesAsync()
    {
        var newPhotos = await _photoService.SelectImagesAsync();
        if (newPhotos.Any())
        {
            foreach (var photo in newPhotos)
            {
                UploadedPhotos.Add(photo);
            }
            UpdatePhotosSummary();
        }
    }

    public void RemovePhoto(UploadedPhoto photo)
    {
        UploadedPhotos.Remove(photo);
        _photoService.DisposeImagePreview(photo);
        UpdatePhotosSummary();
    }

    private void UpdatePhotosSummary()
    {
        if (!UploadedPhotos.Any())
        {
            PhotosSummary = "Фотографии не добавлены";
            return;
        }

        var totalSize = UploadedPhotos.Sum(p => p.FileSize);
        PhotosSummary = $"{UploadedPhotos.Count} фото • {_photoService.FormatFileSize(totalSize)}";
    }
    
    private void ClearForm()
    {
        NewAddress = new Address();
        NewListing = new Listing();
        NewClient = new Client();
        NewArea = new Area();
        NewApartament = new Apartment();
        NewPrivateHouse = new PrivateHouse();
        UploadedPhotos.Clear();
        UpdatePhotosSummary();
        
        // Сбрасываем выбранные типы ремонта и санузлов
        foreach (var item in RenovationTypes)
        {
            item.IsSelected = false;
        }
        foreach (var item in BathroomTypes)
        {
            item.IsSelected = false;
        }
    }
}