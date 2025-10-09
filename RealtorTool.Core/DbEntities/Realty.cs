namespace RealtorTool.Core.DbEntities;

public class Realty : BaseIdEntity
{
    public string Name { get; set; } = null!;
    
    public string? Description { get; set; }
    
    public string AddressId { get; set; } = null!;
    
    public Address Address { get; set; } = null!;
    
    public string? ParentRealtyId { get; set; }
    
    public Realty? ParentRealty { get; set; }

    public ICollection<Realty>? ChildRealties { get; set; } = new List<Realty>();
    
    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}