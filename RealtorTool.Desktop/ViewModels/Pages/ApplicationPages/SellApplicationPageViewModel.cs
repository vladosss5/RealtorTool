using System;
using System.Collections.Generic;
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
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;

namespace RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

public class SellApplicationPageViewModel : ViewModelBase
{
    private readonly DataContext _context;

    [Reactive] public Address NewAddress { get; set; } = new();
    [Reactive] public Listing NewListing { get; set; } = new();
    [Reactive] public Client NewClient { get; set; } = new();
    [Reactive] public Area NewArea { get; set; } = new();
    [Reactive] public Apartment NewApartament { get; set; } = new();

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
    
    public ICommand CreateSellRequest { get; }

    public SellApplicationPageViewModel(DataContext context)
    {
        _context = context;
        CreateSellRequest = ReactiveCommand.CreateFromTask(CreateSellRequestAsync);

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

            switch (CurrentRealtyType)
            {
                case RealtyType.Area:
                    await AddAreaDataToContextAsync();
                    break;
                case RealtyType.Apartment:
                    await AddApartmentDataToContextAsync();
                    NewListing.Realty = NewApartament;
                    break;
                case RealtyType.PrivateHouse:
                    await AddPrivateHouseDataToContextAsync();
                    break;
            }
            
            NewListing.Owner = NewClient;
            NewListing.CurrencyId = "currency_rub";
            NewListing.ListingTypeId = "listing_sale";
            NewListing.StatusId = "listing_active";

            _context.Add(NewListing);
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

    private async Task AddApartmentDataToContextAsync()
    {
        NewApartament.RenovationType = RenovationTypes.FirstOrDefault(x => x.IsSelected);
        NewApartament.BathroomType = BathroomTypes.FirstOrDefault(x => x.IsSelected);
        NewApartament.Address = NewAddress;
        await _context.AddAsync(NewApartament);
    }

    private async Task AddAreaDataToContextAsync()
    {
        throw new NotImplementedException();
    }

    private async Task AddClientDataToContextAsync()
    {
        await _context.AddAsync(NewClient);
    }
}