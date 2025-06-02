using System.ComponentModel.DataAnnotations;

namespace RealtorTool.Core.DbModels;

/// <summary>
/// Базовый класс с Id.
/// </summary>
public class BaseIdModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    [Required]
    public string? Id { get; set; }
}