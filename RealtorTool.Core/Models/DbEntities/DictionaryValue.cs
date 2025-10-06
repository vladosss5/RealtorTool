namespace RealtorTool.Core.Models.DbEntities;

/// <summary>
/// Словари значения.
/// </summary>
public partial class DictionaryValue : IdModelBase
{
    public string Dictionaryid { get; set; }

    public string Value { get; set; } = null!;

    public virtual Dictionary Dictionary { get; set; } = null!;

    public virtual ICollection<Realty> Realties { get; set; } = new List<Realty>();

    public virtual ICollection<Request> Requests { get; set; } = new List<Request>();
}
