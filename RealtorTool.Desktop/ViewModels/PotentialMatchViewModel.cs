using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.DbEntities.Views;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;

namespace RealtorTool.Desktop.ViewModels;

public class PotentialMatchViewModel : ViewModelBase
{
    private readonly PotentialMatch _match;
    private readonly DataContext _context;

    public PotentialMatchViewModel(PotentialMatch match, DataContext context)
    {
        _match = match;
        _context = context;
    }

    public string BuyRequestId => _match.BuyRequestId;
    public string SellRequestId => _match.SellRequestId;
    public decimal MatchScore => _match.MatchScore;
    public string MatchDescription => _match.MatchDescription;
    public bool IsGoodMatch => _match.IsGoodMatch;
    
    public string RealtyTypeDisplay => _match.RealtyType switch
    {
        RealtyType.Apartment => "Квартира",
        RealtyType.PrivateHouse => "Частный дом", 
        RealtyType.Area => "Земельный участок",
        _ => "Неизвестно"
    };

    public string PriceInfo => _match.ListingPrice.ToString("N0") + " руб.";

    public async Task<Realty?> GetRealtyAsync()
    {
        var sellRequest = await _context.ClientRequests
            .Include(cr => cr.Listing)
            .ThenInclude(l => l.Realty)
            .FirstOrDefaultAsync(cr => cr.Id == _match.SellRequestId);
            
        return sellRequest?.Listing?.Realty;
    }

    public async Task<Client?> GetSellerAsync()
    {
        var sellRequest = await _context.ClientRequests
            .Include(cr => cr.Client)
            .FirstOrDefaultAsync(cr => cr.Id == _match.SellRequestId);
            
        return sellRequest?.Client;
    }
}