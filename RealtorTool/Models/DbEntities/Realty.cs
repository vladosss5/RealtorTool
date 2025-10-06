using System;
using System.Collections.Generic;

namespace RealtorTool.Models.DbEntities;

/// <summary>
/// Недвижимость
/// </summary>
public partial class Realty
{
    public int Id { get; set; }

    public int? Typeid { get; set; }

    public string Name { get; set; } = null!;

    public string? Realty1 { get; set; }

    public virtual ICollection<RealtyAttribute> RealtyAttributes { get; set; } = new List<RealtyAttribute>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();

    public virtual DictionaryValue? Type { get; set; }
}
