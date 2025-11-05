namespace RealtorTool.Core.DbEntities;

public class Address : BaseIdEntity, ISoftDelete
{
    public string? City { get; set; }

    public string? District { get; set; }

    public string Street { get; set; } = null!;

    public string? HouseNumber { get; set; }

    public string? BuildingNumber { get; set; }

    public string? PostalCode { get; set; }
    
    public bool IsDeleted { get; set; }
}