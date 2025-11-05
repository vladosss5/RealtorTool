namespace RealtorTool.Core.DbEntities;

public class Deal : BaseIdEntity, ISoftDelete
{
    public string ListingId { get; set; } = null!;
    
    public Listing Listing { get; set; } = null!;
    
    public string BuyerId { get; set; } = null!;
    
    public Client Buyer { get; set; } = null!;
    
    public string? EmployeeId { get; set; }
    
    public Employee? Employee { get; set; }
    
    public decimal FinalPrice { get; set; }
    
    public decimal Commission { get; set; }
    
    public DateTime DealDate { get; set; }
    
    public string? DealTypeId { get; set; }
    
    public DictionaryValue? DealType { get; set; }
    
    public string StatusId { get; set; } = null!;
    
    public DictionaryValue Status { get; set; }
    
    public ICollection<DealParticipant> Participants { get; set; } = new List<DealParticipant>();
    
    public bool IsDeleted { get; set; }
}