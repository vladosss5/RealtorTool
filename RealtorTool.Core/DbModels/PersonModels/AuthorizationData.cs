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
    /// Пароль пользователя
    /// </summary>
    public string Password { get; set; }
}