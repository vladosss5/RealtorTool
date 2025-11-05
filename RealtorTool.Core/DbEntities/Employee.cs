using System.ComponentModel.DataAnnotations.Schema;

namespace RealtorTool.Core.DbEntities;

public class Employee : BaseIdEntity, ISoftDelete
{
    public string Login { get; set; } = null!;
    
    public string PasswordHash { get; set; } = null!;

    public string? Salt { get; set; }
    
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string? MiddleName { get; set; }
    
    public DateTime? LastAuthentication { get; set; }
    
    public string? RoleId { get; set; }
    
    public DictionaryValue? Role { get; set; }
    
    public string? PhotoId { get; set; }
    
    public Photo? Photo { get; set; }
    
    public bool IsDeleted { get; set; }
    
    public bool Fired { get; set; }
    
    [NotMapped]
    public bool HasPhoto => Photo != null;
    
    [NotMapped]
    public string FullName => $"{LastName} {FirstName} {MiddleName}";
}