namespace RealtorTool.Core.Models.DbModels;

public partial class ClientRequest
{
    public int RequestId { get; set; }

    public int ClientId { get; set; }

    public string RequestType { get; set; } = null!;

    public string PropertyType { get; set; } = null!;

    public decimal? MinArea { get; set; }

    public decimal? MaxArea { get; set; }

    public int? MinRooms { get; set; }

    public int? MaxRooms { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public List<string>? PreferredDistricts { get; set; }

    public int? MinFloor { get; set; }

    public int? MaxFloor { get; set; }

    public bool? HasBalcony { get; set; }

    public bool? HasParking { get; set; }

    public string? OtherPreferences { get; set; }

    public DateOnly? RequestDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? EmployeeId { get; set; }

    public virtual ICollection<AssignmentHistory> AssignmentHistories { get; set; } = new List<AssignmentHistory>();

    public virtual Client Client { get; set; } = null!;

    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    public virtual Employee? Employee { get; set; }
}
