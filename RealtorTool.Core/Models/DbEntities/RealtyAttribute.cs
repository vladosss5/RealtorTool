namespace RealtorTool.Core.Models.DbEntities;

/// <summary>
/// Связь недвижимости и атрибутов
/// </summary>
public partial class RealtyAttribute
{
    public string IdRealty { get; set; }

    public string IdAttribute { get; set; }

    public string? Value { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual Realty Realty { get; set; } = null!;
}
