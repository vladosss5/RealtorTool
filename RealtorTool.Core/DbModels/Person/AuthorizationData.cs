using RealtorTool.Core.DbModels.Person;

namespace RealtorTool.Core.DbModels;

public class AuthorizationData : BaseIdModel
{
    public string Login { get; set; }
    
    public string Password { get; set; }
}