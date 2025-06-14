namespace RealtorTool.Core.DbModels.PersonModels;

/// <summary>
/// Данные авторизации.
/// </summary>
public class AuthorizationData : BaseIdModel
{
    /// <summary>
    /// Логин пользователя
    /// </summary>
    public string Login { get; set; }

    /// <summary>
    /// Хеш пароля
    /// </summary>
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// Соль для хеширования
    /// </summary>
    public string Salt { get; set; } = null!;
    
    /// <summary>
    /// Пользователь
    /// </summary>
    public Person? Person { get; set; }
}