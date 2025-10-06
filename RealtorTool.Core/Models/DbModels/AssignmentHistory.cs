namespace RealtorTool.Core.Models.DbModels;

public partial class AssignmentHistory
{
    public int AssignmentId { get; set; }

    public int EmployeeId { get; set; }

    public int? ListingId { get; set; }

    public int? RequestId { get; set; }

    public int? DealId { get; set; }

    public DateTime? AssignedDate { get; set; }

    public DateTime? UnassignedDate { get; set; }

    public string AssignmentType { get; set; } = null!;

    public string? Notes { get; set; }

    public virtual Deal? Deal { get; set; }

    public virtual Employee Employee { get; set; } = null!;

    public virtual PropertyListing? Listing { get; set; }

    public virtual ClientRequest? Request { get; set; }
}
