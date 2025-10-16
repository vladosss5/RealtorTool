namespace RealtorTool.Core.DbEntities;

public class Listing : BaseIdEntity
{
    public string RealtyId { get; set; } = null!;
    
    public Realty Realty { get; set; } = null!;
    
    public string OwnerId { get; set; } = null!;
    
    public Client Owner { get; set; } = null!;
    
    public string? ResponsibleEmployeeId { get; set; }
    
    public Employee? ResponsibleEmployee { get; set; }
    
    public decimal Price { get; set; }
    
    public string? CurrencyId { get; set; }
    
    public DictionaryValue? Currency { get; set; }
    
    public string? ListingTypeId { get; set; }
    
    public DictionaryValue ListingType { get; set; } = null!;
    
    public string? StatusId { get; set; }
    
    public DictionaryValue? Status { get; set; }

    public string? Terms { get; set; }
    
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    public ICollection<ClientRequest> ClientRequests { get; set; } = new List<ClientRequest>();
}