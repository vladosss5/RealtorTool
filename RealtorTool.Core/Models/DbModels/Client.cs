namespace RealtorTool.Core.Models.DbModels;

public partial class Client
{
    public int ClientId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Phone { get; set; } = null!;

    public string? Email { get; set; }

    public string? PassportSeries { get; set; }

    public string? PassportNumber { get; set; }

    public int? RegistrationAddressId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ClientRequest> ClientRequests { get; set; } = new List<ClientRequest>();

    public virtual ICollection<Deal> DealBuyers { get; set; } = new List<Deal>();

    public virtual ICollection<Deal> DealSellers { get; set; } = new List<Deal>();

    public virtual ICollection<PropertyListing> PropertyListings { get; set; } = new List<PropertyListing>();

    public virtual Address? RegistrationAddress { get; set; }
}
