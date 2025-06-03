namespace RealtorTool.Core.DbModels.PersonModels;

/// <summary>
/// Данные о человеке.
/// </summary>
public class Person : BaseIdModel
{
    /// <summary>
    /// Имя.
    /// </summary>
    public string FName { get; set; } = null!;

    /// <summary>
    /// Фамилия.
    /// </summary>
    public string SName { get; set; } = null!;

    /// <summary>
    /// Отчество
    /// </summary>
    public string? LName { get; set; }
    
    /// <summary>
    /// Номер телефона.
    /// </summary>
    public string? PhoneNumber { get; set; }
    
    /// <summary>
    /// Адрес электронной почты.
    /// </summary>
    public string? EMail { get; set; }
    
    /// <summary>
    /// Ссылка на роль человека в системе.
    /// </summary>
    public DictionaryValue Role { get; set; } = null!;
    
    /// <summary>
    /// Данные авторизации.
    /// </summary>
    public AuthorizationData? AuthorizationData { get; set; }
    
    /// <summary>
    /// Данные риелтора.
    /// </summary>
    public RealtorDetails? RealtorDetails { get; set; }
}