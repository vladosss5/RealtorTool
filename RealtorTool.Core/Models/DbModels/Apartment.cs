namespace RealtorTool.Core.Models.DbModels;

public partial class Apartment
{
    public int ApartmentId { get; set; }

    public int BuildingId { get; set; }

    public string? ApartmentNumber { get; set; }

    public int? Floor { get; set; }

    public int? RoomsCount { get; set; }

    public decimal? TotalArea { get; set; }

    public decimal? LivingArea { get; set; }

    public decimal? KitchenArea { get; set; }

    public bool? HasBalcony { get; set; }

    public bool? HasLoggia { get; set; }

    public string? RenovationType { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Building Building { get; set; } = null!;

    public virtual ICollection<PropertyListing> PropertyListings { get; set; } = new List<PropertyListing>();
}
