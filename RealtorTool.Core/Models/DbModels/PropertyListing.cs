namespace RealtorTool.Core.Models.DbModels;

public partial class PropertyListing
{
    public int ListingId { get; set; }

    public int ClientId { get; set; }

    public string TransactionType { get; set; } = null!;

    public decimal? Price { get; set; }

    public string? Currency { get; set; }

    public decimal? Commission { get; set; }

    public bool? IsExclusive { get; set; }

    public DateOnly? ListingDate { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    public int? ApartmentId { get; set; }

    public int? PrivateHouseId { get; set; }

    public int? LandPlotId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? EmployeeId { get; set; }

    public virtual Apartment? Apartment { get; set; }

    public virtual ICollection<AssignmentHistory> AssignmentHistories { get; set; } = new List<AssignmentHistory>();

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    public virtual Employee? Employee { get; set; }

    public virtual LandPlot? LandPlot { get; set; }

    public virtual PrivateHouse? PrivateHouse { get; set; }
}
