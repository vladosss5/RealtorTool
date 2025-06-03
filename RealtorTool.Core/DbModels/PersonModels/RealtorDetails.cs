namespace RealtorTool.Core.DbModels.PersonModels;

/// <summary>
/// Данные риелтора.
/// </summary>
public class RealtorDetails : BaseIdModel
{
    /// <summary>
    /// Опыт работы.
    /// </summary>
    public string? WorkExperience { get; set; }
    
    /// <summary>
    /// Образование.
    /// </summary>
    public string? Education  { get; set; }
    
    /// <summary>
    /// Квалификация.
    /// </summary>
    public string? Qualification { get; set; }
    
    /// <summary>
    /// Предоставляемые услуги.
    /// </summary>
    public string? Services { get; set; }
    
    /// <summary>
    /// Обо мне.
    /// </summary>
    public string? AboutMe { get; set; }
}