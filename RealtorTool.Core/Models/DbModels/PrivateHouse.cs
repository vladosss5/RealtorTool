namespace RealtorTool.Core.Models.DbModels;

public partial class PrivateHouse
{
    public int PrivateHouseId { get; set; }

    public int LandPlotId { get; set; }

    public int? RoomsCount { get; set; }

    public decimal? TotalArea { get; set; }

    public int? FloorsCount { get; set; }

    public bool? HasGarage { get; set; }

    public bool? HasBasement { get; set; }

    public string? HeatingType { get; set; }

    public string? ConstructionMaterial { get; set; }

    public int? YearBuilt { get; set; }

    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual LandPlot LandPlot { get; set; } = null!;

    public virtual ICollection<PropertyListing> PropertyListings { get; set; } = new List<PropertyListing>();
}
