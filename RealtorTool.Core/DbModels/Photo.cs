using System.ComponentModel.DataAnnotations;

namespace RealtorTool.Core.DbModels;

/// <summary>
/// Фото.
/// </summary>
public class Photo : BaseIdModel
{
    /// <summary>
    /// Изображение в виде массива байт
    /// </summary>
    [Required]
    public byte[] ImageData { get; set; }
}