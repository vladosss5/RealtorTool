using System.Reactive;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Data.Context;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class ListingDetailViewModel : PageViewModelBase
{
    private readonly DataContext _context;
    
    [Reactive] public Listing? CurrentListing { get; set; }
    

    public ListingDetailViewModel(DataContext context)
    {
        _context = context;
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
        CurrentListing = await _context.Listings
            .Include(l => l.Realty)
            .Include(l => l.ListingType)
            .Include(l => l.Status)
            .Include(l => l.Currency)
            .Include(l => l.Owner)
            .Include(l => l.ResponsibleEmployee)
            .FirstOrDefaultAsync(l => l.Id == listingId);
    }
}