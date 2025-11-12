using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorTool.Core.DbEntities;

public class Client : BaseIdEntity, ISoftDelete
{
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }

    public string Phone { get; set; } = null!;
    
    public string? PassportSeries { get; set; }

    public string? PassportNumber { get; set; }
    
    public string? RegistrationAddress { get; set; }
    
    public string? PhotoId { get; set; }
    
    public Photo? Photo { get; set; }
    
    public bool IsDeleted { get; set; }
    
    [NotMapped]
    public string FullName => $"{FirstName} {LastName}";
}