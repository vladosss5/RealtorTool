namespace RealtorTool.Core.DbEntities;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}