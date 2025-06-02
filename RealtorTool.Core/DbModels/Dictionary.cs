using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace RealtorTool.Core.DbModels;

/// <summary>
/// Тип словаря.
/// </summary>
public class Dictionary : BaseIdModel
{
    /// <summary>
    /// Значения справочника данного типа.
    /// </summary>
    public ICollection<DictionaryValue>? DictionaryValues { get; set; } = new Collection<DictionaryValue?>()!;

    /// <summary>
    /// Наименование типа справочника.
    /// </summary>
    [Required]
    public string Type { get; set; } = null!;
}