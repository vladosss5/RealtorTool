namespace RealtorTool.Core.DbEntities;

public class Employee : BaseIdEntity
{
    public string Login { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;

    public string? Salt { get; set; }
    
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }
    
    public string? PhotoId { get; set; }
    
    public Photo? Photo { get; set; }
}