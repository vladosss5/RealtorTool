namespace RealtorTool.Core.Models.DbEntities;

/// <summary>
/// Клиенты.
/// </summary>
public partial class Client : IdModelBase
{
    public string Fname { get; set; } = null!;

    public string Sname { get; set; } = null!;

    public string? Lname { get; set; }

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
