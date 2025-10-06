namespace RealtorTool.Core.Models.DbEntities;

/// <summary>
/// Словари.
/// </summary>
public partial class Dictionary : IdModelBase
{
    public string Type { get; set; } = null!;

    public virtual ICollection<DictionaryValue> DictionaryValues { get; set; } = new List<DictionaryValue>();
}
