using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class ListingDetailViewModel : PageViewModelBase
{
    private readonly DataContext _context;
    
    [Reactive] public Listing? Listing { get; set; }
    [Reactive] public PageViewModelBase RealtyViewModel { get; set; }
    
    public string OwnerFullName => Listing is null ? string.Empty :
        $"{Listing.Owner?.FirstName} {Listing.Owner?.LastName}".Trim();

    public string EmployeeFullName => Listing is null ? string.Empty :
        $"{Listing.Owner?.FirstName} {Listing.Owner?.LastName}".Trim();

    public ListingDetailViewModel(DataContext context)
    {
        _context = context;
        
        MessageBus.Current
            .Listen<string>("ListingDetailsId")
            .Subscribe(listingId =>
            {
                Listing = _context.Listings
                    .Include(x => x.Realty)
                    .FirstOrDefault(x => x.Id == listingId);
                Title = $"Заявка №{listingId}";
                LoadRealtyView();
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
        Listing = await _context.Listings
            .Include(l => l.Realty)
            .Include(l => l.ListingType)
            .Include(l => l.Status)
            .Include(l => l.Currency)
            .Include(l => l.Owner)
            .Include(l => l.ResponsibleEmployee)
            .FirstOrDefaultAsync(l => l.Id == listingId);
    }
    
    private void LoadRealtyView()
    {
        var realty = Listing.Realty;
        
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
                RealtyViewModel = null!;
                break;
        }
    }
}