namespace RealtorTool.Core.DbEntities;

public class Area : Realty
{
    public decimal Square { get; set; }
    
    public string? LandCategoryId { get; set; }
    
    public DictionaryValue? LandCategory { get; set; }
    
    public bool HasUtilities { get; set; }
}