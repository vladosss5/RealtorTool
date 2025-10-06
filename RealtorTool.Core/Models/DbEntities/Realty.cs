namespace RealtorTool.Core.Models.DbEntities;

/// <summary>
/// Недвижимость
/// </summary>
public partial class Realty : IdModelBase
{
    public string TypeId { get; set; }
    public string Name { get; set; } = null!;
    
    // СТРУКТУРИРОВАННЫЕ ПОЛЯ ДЛЯ ПОИСКА:
    public string Address { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "RUB";
    public string StatusId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
    
    // JSON ДЛЯ СПЕЦИФИЧЕСКИХ АТРИБУТОВ:
    public string? Attributes { get; set; }
    
    // Навигационные свойства
    public virtual DictionaryValue Type { get; set; } = null!;
    public virtual DictionaryValue Status { get; set; } = null!;
    
    public virtual ICollection<RealtyRequest> RequestsLink { get; set; } = new List<RealtyRequest>();
    
    public virtual ICollection<RealtyAttribute> RealtyAttributes { get; set; } = new List<RealtyAttribute>();
}