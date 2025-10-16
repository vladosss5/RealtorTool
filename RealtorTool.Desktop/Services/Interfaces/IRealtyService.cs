using System.Collections.Generic;
using System.Threading.Tasks;
using RealtorTool.Core.DbEntities;

namespace RealtorTool.Desktop.Services.Interfaces;

public interface IRealtyService
{
    /// <summary>
    /// Получить объект недвижимости по идентификатору
    /// </summary>
    public Task<Realty?> GetByIdAsync(string id);
    
    /// <summary>
    /// Создать новый объект недвижимости
    /// </summary>
    public Task<Realty> CreateAsync(Realty realty);
    
    /// <summary>
    /// Обновить объект недвижимости
    /// </summary>
    public Task UpdateAsync(Realty realty);
    
    /// <summary>
    /// Удалить объект недвижимости
    /// </summary>
    public Task DeleteAsync(string id);
    
    /// <summary>
    /// Получить все активные объекты недвижимости
    /// </summary>
    public Task<List<Realty>> GetActiveRealtiesAsync();
}