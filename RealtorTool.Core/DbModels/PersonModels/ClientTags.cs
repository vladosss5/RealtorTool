namespace RealtorTool.Core.DbModels.PersonModels;

/// <summary>
/// Теги клиентов.
/// </summary>
public class ClientTags : BaseIdModel
{
    /// <summary>
    /// Идентификатор клиента
    /// </summary>
    public string ClientId { get; set; }
    
    /// <summary>
    /// Клиент (объект Person)
    /// </summary>
    public Person Client { get; set; }
    
    /// <summary>
    /// Идентификатор тега
    /// </summary>
    public string TagId { get; set; }
    
    /// <summary>
    /// Тег (справочное значение)
    /// </summary>
    public DictionaryValue Tag { get; set; }
}