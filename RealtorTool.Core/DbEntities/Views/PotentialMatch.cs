using RealtorTool.Core.Enums;

namespace RealtorTool.Core.DbEntities.Views;

public class PotentialMatch
{
    public string BuyRequestId { get; set; }
    public string SellRequestId { get; set; }
    public string BuyerId { get; set; }
    public string SellerId { get; set; }
    public decimal? MaxPrice { get; set; }
    public decimal ListingPrice { get; set; }
    public string RealtyId { get; set; }
    public string SellListingId { get; set; }
    public RealtyType RealtyType { get; set; }
    public decimal? TotalArea { get; set; }
    public string RealtyName { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int? MinRooms { get; set; }
    public decimal? MinArea { get; set; }
    public decimal? MaxArea { get; set; }
    public string DesiredLocation { get; set; }
    public int? DesiredRealtyType { get; set; }
    public int? ActualRoomsCount { get; set; }
    public bool TypeMatch { get; set; }
    public int MatchScore { get; set; }
    public bool PriceMatch { get; set; }
    public bool MinAreaMatch { get; set; }
    public bool MaxAreaMatch { get; set; }
    public bool RoomsMatch { get; set; }
    public bool LocationMatch { get; set; }
    public ApplicationType BuyType { get; set; }
    public ApplicationType SellType { get; set; }
    public bool IsGoodMatch => MatchScore >= 70;
    public bool IsPerfectMatch => MatchScore >= 90;
    public decimal? PriceDifference => MaxPrice.HasValue ? ListingPrice - MaxPrice.Value : null;
    public bool IsWithinBudget => !MaxPrice.HasValue || ListingPrice <= MaxPrice.Value;
    
    public string RealtyTypeDisplayName => RealtyType switch
    {
        RealtyType.Apartment => "Квартира",
        RealtyType.PrivateHouse => "Частный дом",
        RealtyType.Area => "Земельный участок",
        _ => "Неизвестный тип"
    };
    
    public string MatchDescription
    {
        get
        {
            if (MatchScore >= 90) return "★ Идеальное совпадение";
            if (MatchScore >= 70) return "✓ Хорошее совпадение";
            if (MatchScore >= 50) return "○ Возможное совпадение";
            return "△ Слабое совпадение";
        }
    }
}