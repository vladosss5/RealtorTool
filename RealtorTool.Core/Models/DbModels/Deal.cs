namespace RealtorTool.Core.Models.DbModels;

public partial class Deal
{
    public int DealId { get; set; }

    public int ListingId { get; set; }

    public int? RequestId { get; set; }

    public int BuyerId { get; set; }

    public int SellerId { get; set; }

    public DateOnly? DealDate { get; set; }

    public decimal FinalPrice { get; set; }

    public decimal? CommissionAmount { get; set; }

    public string? DealStatus { get; set; }

    public string? ContractNumber { get; set; }

    public string? Notes { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? EmployeeId { get; set; }

    public virtual ICollection<AssignmentHistory> AssignmentHistories { get; set; } = new List<AssignmentHistory>();

    public virtual Client Buyer { get; set; } = null!;

    public virtual Employee? Employee { get; set; }

    public virtual PropertyListing Listing { get; set; } = null!;

    public virtual ClientRequest? Request { get; set; }

    public virtual Client Seller { get; set; } = null!;
}
