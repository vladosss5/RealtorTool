namespace RealtorTool.Core.DbModels;

/// <summary>
/// Адрес.
/// </summary>
public class Address : BaseIdModel
{
    /// <summary>
    /// Регион (область, край)
    /// </summary>
    public DictionaryValue? Region { get; set; }
    
    /// <summary>
    /// Город
    /// </summary>
    public DictionaryValue? City { get; set; }
    
    /// <summary>
    /// Район города
    /// </summary>
    public DictionaryValue? District { get; set; }
    
    /// <summary>
    /// Улица
    /// </summary>
    public DictionaryValue? Street { get; set; }
    
    /// <summary>
    /// Номер дома
    /// </summary>
    public string? HouseNumber { get; set; }
}