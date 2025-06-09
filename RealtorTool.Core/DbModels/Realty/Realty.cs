namespace RealtorTool.Core.DbModels.Realty;

/// <summary>
/// Недвижимость.
/// </summary>
public class Realty : BaseIdModel
{
    /// <summary>
    /// Фотографии объекта
    /// </summary>
    public ICollection<Photo> Photos { get; set; } = new List<Photo>();
    
    /// <summary>
    /// Идентификатор типа недвижимости
    /// </summary>
    public string TypeId { get; set; }
    
    /// <summary>
    /// Тип недвижимости (справочное значение)
    /// </summary>
    public DictionaryValue Type { get; set; }
    
    /// <summary>
    /// Идентификатор заявки
    /// </summary>
    public string ApplicationId { get; set; }
    
    /// <summary>
    /// Заявка на объект недвижимости
    /// </summary>
    public Application Application { get; set; }
}