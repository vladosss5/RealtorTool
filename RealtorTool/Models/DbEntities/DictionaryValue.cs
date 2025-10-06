using System;
using System.Collections.Generic;

namespace RealtorTool.Models.DbEntities;

/// <summary>
/// Словари значения.
/// </summary>
public partial class DictionaryValue
{
    public int Id { get; set; }

    public int Dictionaryid { get; set; }

    public string Value { get; set; } = null!;

    public virtual Dictionary Dictionary { get; set; } = null!;

    public virtual ICollection<Realty> Realties { get; set; } = new List<Realty>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
