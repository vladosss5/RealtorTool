using System;
using System.Collections.Generic;

namespace RealtorTool.Models.DbEntities;

/// <summary>
/// Связь недвижимости и атрибутов
/// </summary>
public partial class RealtyAttribute
{
    public int IdRealty { get; set; }

    public int IdAttribute { get; set; }

    public string? Value { get; set; }

    public virtual Attribute Attribute { get; set; } = null!;

    public virtual Realty Realty { get; set; } = null!;
}
