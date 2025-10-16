using RealtorTool.Core.Enums;

namespace RealtorTool.Core.DbEntities;

/// <summary>
/// Запрос клиента на операции с недвижимостью
/// </summary>
public class ClientRequest : BaseIdEntity
{
    /// <summary>
    /// Тип запроса (аренда, сдача, покупка, продажа)
    /// </summary>
    public ApplicationType Type { get; set; }
    
    /// <summary>
    /// Текущий статус запроса
    /// </summary>
    public ApplicationStatus Status { get; set; }
    
    /// <summary>
    /// Дата создания запроса
    /// </summary>
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    
    /// <summary>
    /// Дата завершения запроса
    /// </summary>
    public DateTime? CompletedDate { get; set; }
    
    /// <summary>
    /// Идентификатор клиента-инициатора запроса
    /// </summary>
    public string ClientId { get; set; } = null!;
    
    /// <summary>
    /// Клиент-инициатор запроса
    /// </summary>
    public Client Client { get; set; } = null!;
    
    /// <summary>
    /// Идентификатор сотрудника, сопровождающего запрос
    /// </summary>
    public string EmployeeId { get; set; } = null!;
    
    /// <summary>
    /// Сотрудник, сопровождающий запрос
    /// </summary>
    public Employee Employee { get; set; } = null!;
    
    // Параметры поиска для запросов на покупку/аренду
    /// <summary>
    /// Максимальная цена для запросов на покупку/аренду
    /// </summary>
    public decimal? MaxPrice { get; set; }
    
    /// <summary>
    /// Минимальное количество комнат
    /// </summary>
    public int? MinRooms { get; set; }
    
    /// <summary>
    /// Минимальная площадь
    /// </summary>
    public decimal? MinArea { get; set; }
    
    /// <summary>
    /// Максимальная площадь
    /// </summary>
    public decimal? MaxArea { get; set; }
    
    /// <summary>
    /// Желаемое расположение
    /// </summary>
    public string? DesiredLocation { get; set; }
    
    /// <summary>
    /// Дополнительные требования клиента
    /// </summary>
    public string? AdditionalRequirements { get; set; }
    
    // Ссылка на созданный Listing для запросов на продажу/сдачу
    /// <summary>
    /// Идентификатор объявления (для запросов на продажу/сдачу)
    /// </summary>
    public string? ListingId { get; set; }
    
    /// <summary>
    /// Объявление связанное с запросом
    /// </summary>
    public Listing? Listing { get; set; }
    
    /// <summary>
    /// Идентификатор встречного запроса при успешном matching
    /// </summary>
    public string? MatchedRequestId { get; set; }
    
    /// <summary>
    /// Встречный запрос
    /// </summary>
    public ClientRequest? MatchedRequest { get; set; }
    
    /// <summary>
    /// Идентификатор сделки
    /// </summary>
    public string? DealId { get; set; }
    
    /// <summary>
    /// Сделка по запросу
    /// </summary>
    public Deal? Deal { get; set; }
    
    /// <summary>
    /// Идентификатор объекта недвижимости
    /// </summary>
    public string? RealtyId { get; set; }
    
    /// <summary>
    /// Объект недвижимости связанный с запросом
    /// </summary>
    public Realty? Realty { get; set; }
}