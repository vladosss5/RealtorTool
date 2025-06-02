namespace RealtorTool.Core.DbModels.Person;

public class Person : BaseIdModel
{
    public string FName { get; set; }
    
    public string SName { get; set; }
    
    public string? LName { get; set; }
    
    public Role Role { get; set; }
    
    public AuthorizationData AuthorizationData { get; set; }
}