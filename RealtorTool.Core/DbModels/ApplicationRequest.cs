namespace RealtorTool.Core.DbModels;

/// <summary>
/// Пожелания по заявке
/// </summary>
public class ApplicationRequest : BaseIdModel
{
    /// <summary>
    /// Город
    /// </summary>
    public DictionaryValue? City { get; set; }
    
    /// <summary>
    /// Район города
    /// </summary>
    public DictionaryValue? District { get; set; }
    
    /// <summary>
    /// Тип недвижимости (справочное значение)
    /// </summary>
    public DictionaryValue? Type { get; set; }
}