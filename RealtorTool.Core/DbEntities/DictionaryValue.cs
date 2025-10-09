namespace RealtorTool.Core.DbEntities;

public class DictionaryValue : BaseIdEntity
{
    public string DictionaryId { get; set; } = null!;
    
    public Dictionary? Dictionary { get; set; }
    
    public string Value { get; set; } = null!;
}