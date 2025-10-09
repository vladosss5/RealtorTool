namespace RealtorTool.Core.DbEntities;

public class Apartment : Realty
{
    public string? ApartmentNumber { get; set; }

    public int? Floor { get; set; }

    public int? RoomsCount { get; set; }

    public decimal? TotalArea { get; set; }

    public decimal? LivingArea { get; set; }

    public decimal? KitchenArea { get; set; }

    public bool? HasBalcony { get; set; }

    public bool? HasLoggia { get; set; }

    public string RenovationTypeId { get; set; }
    
    public DictionaryValue? RenovationType { get; set; }
    
    public string BathroomTypeId { get; set; }
    
    public DictionaryValue? BathroomType { get; set; }
}