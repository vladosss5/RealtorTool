namespace RealtorTool.Core.DbEntities;

public class EmployeePhoto : Photo
{
    public string EmployeeId { get; set; } = null!;
    public Employee Employee { get; set; } = null!;
}