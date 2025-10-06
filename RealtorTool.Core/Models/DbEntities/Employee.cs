namespace RealtorTool.Core.Models.DbEntities;

/// <summary>
/// Сотрудники
/// </summary>
public partial class Employee : IdModelBase
{
    public string Fname { get; set; } = null!;

    public string Sname { get; set; } = null!;

    public string? Lname { get; set; }
    
    public string? PhoneNumber { get; set; }
    
    public string? EMail { get; set; }

    public string Login { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;
    
    public string Salt { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
