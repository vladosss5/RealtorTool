namespace RealtorTool.Core.DbModels.PersonModels;

public class AuthorizationData : BaseIdModel
{
    public string Login { get; set; }
    
    public string Password { get; set; }
}