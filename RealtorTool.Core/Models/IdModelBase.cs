namespace RealtorTool.Core.Models;

public class IdModelBase
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}