namespace RealtorTool.Core.Models.DbEntities;

public class RealtyRequest : IdModelBase
{
    public string RealtyId { get; set; }
    public Realty Realty { get; set; }
    
    public string RequestId { get; set; }
    public Request Request { get; set; }
}