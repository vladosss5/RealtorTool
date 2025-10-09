namespace RealtorTool.Core.DbEntities;

public class BaseIdEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}