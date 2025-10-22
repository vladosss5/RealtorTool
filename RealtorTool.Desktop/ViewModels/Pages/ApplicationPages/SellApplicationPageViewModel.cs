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
    [Reactive] public Apartment NewApartament { get; set; } = new();
    [Reactive] public ObservableCollection<UploadedPhoto> UploadedPhotos { get; set; } = new();
    [Reactive] public string PhotosSummary { get; set; } = "Фотографии не добавлены";

    public bool HasPhotos => UploadedPhotos.Any();

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
            await AddClientDataToContextAsync();
            await AddAddressDataToContextAsync();
            
            Realty? createdRealty = null;

            switch (CurrentRealtyType)
            {
                case RealtyType.Area:
                    await AddAreaDataToContextAsync();
                    break;
                case RealtyType.Apartment:
                    createdRealty = await AddApartmentDataToContextAsync();
                    NewListing.Realty = NewApartament;
                    break;
                case RealtyType.PrivateHouse:
                    await AddPrivateHouseDataToContextAsync();
                    break;
            }
            
            if (createdRealty != null)
            {
                await _photoService.SavePhotosToDatabaseAsync(
                    UploadedPhotos.ToList(), 
                    createdRealty.Id, 
                    EntityTypeForPhoto.Realty);
            }

            NewListing.Realty = createdRealty;
            NewListing.Owner = NewClient;
            NewListing.CurrencyId = "currency_rub";
            NewListing.ListingTypeId = "listing_sale";
            NewListing.StatusId = "listing_active";

            _context.Add(NewListing);
            
            var sellRequest = new ClientRequest
            {
                Type = ApplicationType.Sale,
                Status = ApplicationStatus.New,
                ClientId = NewClient.Id,
                ListingId = NewListing.Id,
                CreatedDate = DateTime.UtcNow
            };
            
            _context.Add(sellRequest);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", e.Message)
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

    private void RemovePhoto(UploadedPhoto photo)
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
}