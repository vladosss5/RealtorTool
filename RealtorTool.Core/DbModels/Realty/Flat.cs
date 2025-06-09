namespace RealtorTool.Core.DbModels.Realty;

/// <summary>
/// Квартира.
/// </summary>
public class Flat : BaseIdModel
{
    /// <summary>
    /// Этаж расположения квартиры
    /// </summary>
    public int Floor { get; set; }
    
    /// <summary>
    /// Высота потолков (м)
    /// </summary>
    public decimal CeilingHeight { get; set; }
    
    /// <summary>
    /// Идентификатор типа санузла
    /// </summary>
    public string BathroomTypeId { get; set; }
    
    /// <summary>
    /// Тип санузла (справочное значение)
    /// </summary>
    public DictionaryValue BathroomType { get; set; }
    
    /// <summary>
    /// Идентификатор состояния ремонта
    /// </summary>
    public string RepairId { get; set; }
    
    /// <summary>
    /// Состояние ремонта (справочное значение)
    /// </summary>
    public DictionaryValue Repair { get; set; }
    
    /// <summary>
    /// Идентификатор типа использования
    /// </summary>
    public string UsedTypeId { get; set; }
    
    /// <summary>
    /// Тип использования (справочное значение)
    /// </summary>
    public DictionaryValue UsedType { get; set; }
    
    /// <summary>
    /// Идентификатор дома
    /// </summary>
    public string HouseId { get; set; }
    
    /// <summary>
    /// Дом, в котором расположена квартира
    /// </summary>
    public House House { get; set; }
}