namespace RealtorTool.Core.DbEntities;

public class Client : BaseIdEntity
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Phone { get; set; } = null!;
    
    public string? PassportSeries { get; set; }

    public string? PassportNumber { get; set; }
    
    public string? RegistrationAddress { get; set; }
    
    
}