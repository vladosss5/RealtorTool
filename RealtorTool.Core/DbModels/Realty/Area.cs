namespace RealtorTool.Core.DbModels.Realty;

/// <summary>
/// Земельный участок.
/// </summary>
public class Area : BaseIdModel
{
    /// <summary>
    /// Общая площадь участка (кв. м)
    /// </summary>
    public decimal TotalSqare { get; set; }
    
    /// <summary>
    /// Наличие дома на участке
    /// </summary>
    public bool HasHouse { get; set; }
    
    /// <summary>
    /// Адрес участка
    /// </summary>
    public Address Address { get; set; }
}