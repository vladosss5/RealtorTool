namespace RealtorTool.Core.DbModels;

/// <summary>
/// Адрес.
/// </summary>
public class Address : BaseIdModel
{
    /// <summary>
    /// Регион (область, край)
    /// </summary>
    public string Region { get; set; }
    
    /// <summary>
    /// Город
    /// </summary>
    public string City { get; set; }
    
    /// <summary>
    /// Район города
    /// </summary>
    public string? District { get; set; }
    
    /// <summary>
    /// Улица
    /// </summary>
    public string? Street { get; set; }
    
    /// <summary>
    /// Номер дома
    /// </summary>
    public string? HouseNumber { get; set; }
}