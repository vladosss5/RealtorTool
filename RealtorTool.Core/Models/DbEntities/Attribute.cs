namespace RealtorTool.Core.Models.DbEntities;

/// <summary>
/// Атрибуты недвижимости.
/// </summary>
public partial class Attribute : IdModelBase
{
    public string Name { get; set; } = null!;

    public virtual ICollection<RealtyAttribute> RealtyAttributes { get; set; } = new List<RealtyAttribute>();
}
