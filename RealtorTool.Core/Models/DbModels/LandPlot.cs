namespace RealtorTool.Core.Models.DbModels;

public partial class LandPlot
{
    public int LandPlotId { get; set; }

    public int AddressId { get; set; }

    public string? CadastralNumber { get; set; }

    public decimal? Area { get; set; }

    public string? ZoningType { get; set; }

    public bool? HasUtilities { get; set; }

    public string? UtilitiesDescription { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<PrivateHouse> PrivateHouses { get; set; } = new List<PrivateHouse>();

    public virtual ICollection<PropertyListing> PropertyListings { get; set; } = new List<PropertyListing>();
}
