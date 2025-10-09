namespace RealtorTool.Core.DbEntities;

public class Address : BaseIdEntity
{
    public string? City { get; set; }

    public string? District { get; set; }

    public string Street { get; set; } = null!;

    public string? HouseNumber { get; set; } = null!;

    public string? BuildingNumber { get; set; }

    public string? PostalCode { get; set; }
}