using RealtorTool.Core.DbModels.PersonModels;

namespace RealtorTool.Core.DbModels;

/// <summary>
/// Заявка.
/// </summary>
public class Application : BaseIdModel
{
    /// <summary>
    /// Номер
    /// </summary>
    public string Number { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Desctiption { get; set; }
    
    /// <summary>
    /// Желаемая стоимость
    /// </summary>
    public decimal DesiredCost { get; set; }
    
    /// <summary>
    /// Id типа
    /// </summary>
    public string TypeDealId { get; set; }
    
    /// <summary>
    /// Навигационка типа сделки
    /// </summary>
    public DictionaryValue TypeDeal { get; set; }
    
    /// <summary>
    /// Id статуса
    /// </summary>
    public string StatusId { get; set; }
    
    /// <summary>
    /// Навигационка статуса
    /// </summary>
    public DictionaryValue Status { get; set; }
    
    /// <summary>
    /// Id клиента
    /// </summary>
    public string ClientId { get; set; }
    
    /// <summary>
    /// Навигационка клиента
    /// </summary>
    public Person Client { get; set; }
    
    /// <summary>
    /// Id риелтора
    /// </summary>
    public string RealtorId { get; set; }
    
    /// <summary>
    /// Навигационка риелтора
    /// </summary>
    public Person Realtor { get; set; }
}