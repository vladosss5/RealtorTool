using System;
using System.Collections.Generic;

namespace RealtorTool.Models.DbEntities;

/// <summary>
/// Атрибуты недвижимости.
/// </summary>
public partial class Attribute
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<RealtyAttribute> RealtyAttributes { get; set; } = new List<RealtyAttribute>();
}
