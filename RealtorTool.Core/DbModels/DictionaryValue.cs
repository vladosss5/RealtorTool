using System.ComponentModel.DataAnnotations;

namespace RealtorTool.Core.DbModels;

/// <summary>
/// Значение словаря.
/// </summary>
public class DictionaryValue : BaseIdModel
{
    /// <summary>
    /// Тип справочника.
    /// </summary>
    [Required]
    public Dictionary Dictionary { get; set; } = null!;

    /// <summary>
    /// Значение справочника.
    /// </summary>
    [Required]
    public string Value { get; set; } = null!;
}