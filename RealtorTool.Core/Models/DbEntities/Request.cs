namespace RealtorTool.Core.Models.DbEntities;

public partial class Request : IdModelBase
{
    public string? ClientId { get; set; }
    public string EmployeeId { get; set; }
    public string TypeId { get; set; }
    
    // СТРУКТУРИРОВАННЫЕ ПОЛЯ:
    public string StatusId { get; set; }
    public int Priority { get; set; } = 1;
    public DateTime CreatedDate { get; set; }
    public DateTime? Deadline { get; set; }
    
    // JSON ДЛЯ КРИТЕРИЕВ И ДАННЫХ:
    public string? RequestData { get; set; }
    
    public virtual Client? Client { get; set; }
    public virtual Employee Employee { get; set; } = null!;
    public virtual DictionaryValue Type { get; set; } = null!;
    public virtual DictionaryValue Status { get; set; } = null!;

    public virtual ICollection<RealtyRequest> RealtyLink { get; set; } = new List<RealtyRequest>();
}