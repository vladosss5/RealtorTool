namespace RealtorTool.Core.Models.DbModels;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Position { get; set; } = null!;

    public string? Department { get; set; }

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public DateOnly HireDate { get; set; }

    public decimal? Salary { get; set; }

    public decimal? CommissionRate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
    
    public string Login { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? Salt { get; set; }

    public virtual ICollection<AssignmentHistory> AssignmentHistories { get; set; } = new List<AssignmentHistory>();

    public virtual ICollection<ClientRequest> ClientRequests { get; set; } = new List<ClientRequest>();

    public virtual ICollection<Deal> Deals { get; set; } = new List<Deal>();

    public virtual ICollection<PropertyListing> PropertyListings { get; set; } = new List<PropertyListing>();
}
