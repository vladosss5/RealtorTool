namespace RealtorTool.Core.Models.DbModels;

public partial class Building
{
    public int BuildingId { get; set; }

    public int AddressId { get; set; }

    public string? Name { get; set; }

    public int? Floors { get; set; }

    public int? YearBuilt { get; set; }

    public string? ConstructionMaterial { get; set; }

    public bool? HasElevator { get; set; }

    public bool? HasParking { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Apartment> Apartments { get; set; } = new List<Apartment>();
}
