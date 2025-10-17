using RealtorTool.Core.Enums;

namespace RealtorTool.Core.DbEntities;

public class Photo : BaseIdEntity
{
    public EntityTypeForPhoto EntityType { get; set; }
    
    public string EntityId { get; set; }
    
    public string FileName { get; set; }
    
    public string ContentType { get; set; }
    
    public int SortOrder { get; set; } = 0;
    
    public bool IsMain { get; set; } = false;
    
    public DateTime CreatedDate { get; set; }
    
    public byte[] FileData { get; set; }
}