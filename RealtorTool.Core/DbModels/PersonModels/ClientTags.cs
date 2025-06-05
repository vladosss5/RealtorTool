namespace RealtorTool.Core.DbModels.PersonModels;

public class ClientTags : BaseIdModel
{
    public string ClientId { get; set; }
    
    public Person Client { get; set; }
    
    public string TagId { get; set; }
    
    public DictionaryValue Tag { get; set; }
}