using System;
using System.Collections.Generic;

namespace RealtorTool.Models.DbEntities;

/// <summary>
/// Словари.
/// </summary>
public partial class Dictionary
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<DictionaryValue> DictionaryValues { get; set; } = new List<DictionaryValue>();
}
