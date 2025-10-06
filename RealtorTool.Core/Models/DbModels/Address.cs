namespace RealtorTool.Core.Models.DbModels;

public partial class Address
{
    public int AddressId { get; set; }

    public string Country { get; set; } = null!;

    public string City { get; set; } = null!;

    public string? District { get; set; }

    public string Street { get; set; } = null!;

    public string HouseNumber { get; set; } = null!;

    public string? BuildingNumber { get; set; }

    public string? ApartmentNumber { get; set; }

    public string? PostalCode { get; set; }

    public decimal? Latitude { get; set; }

    public decimal? Longitude { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Building> Buildings { get; set; } = new List<Building>();

    public virtual ICollection<Client> Clients { get; set; } = new List<Client>();

    public virtual ICollection<LandPlot> LandPlots { get; set; } = new List<LandPlot>();
}
