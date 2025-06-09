namespace RealtorTool.Core.DbModels.Realty;

/// <summary>
/// Частный дом
/// </summary>
public class PrivateHome : BaseIdModel
{
    /// <summary>
    /// Высота потолков (м)
    /// </summary>
    public decimal CeilingHeight { get; set; }
    
    /// <summary>
    /// Общая площадь дома (кв. м)
    /// </summary>
    public decimal Sqare { get; set; }
    
    /// <summary>
    /// Количество комнат
    /// </summary>
    public int RoomCount { get; set; }
    
    /// <summary>
    /// Количество этажей
    /// </summary>
    public int FloorsCount { get; set; }
    
    /// <summary>
    /// Номер дома
    /// </summary>
    public string? AddressNumber { get; set; }
    
    /// <summary>
    /// Идентификатор участка
    /// </summary>
    public string? AreaId { get; set; }
    
    /// <summary>
    /// Участок, на котором расположен дом
    /// </summary>
    public Area? Area { get; set; }
}