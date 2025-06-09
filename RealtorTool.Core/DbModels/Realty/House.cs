namespace RealtorTool.Core.DbModels.Realty;

/// <summary>
/// Многоквартирный дом.
/// </summary>
public class House : BaseIdModel
{
    /// <summary>
    /// Общее количество этажей
    /// </summary>
    public int TotalNumberFloors { get; set; }
    
    /// <summary>
    /// Год постройки
    /// </summary>
    public int YearConstruction { get; set; }
    
    /// <summary>
    /// Идентификатор типа конструкции
    /// </summary>
    public string СonstructionTypeId { get; set; }
    
    /// <summary>
    /// Тип конструкции (справочное значение)
    /// </summary>
    public DictionaryValue СonstructionType { get; set; }
}