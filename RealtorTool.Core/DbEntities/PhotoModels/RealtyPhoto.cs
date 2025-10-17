namespace RealtorTool.Core.DbEntities;

public class RealtyPhoto : Photo
{
    public string RealtyId { get; set; } = null!;
    public Realty Realty { get; set; } = null!;
}