using System.Threading.Tasks;
using RealtorTool.Core.DbEntities;

namespace RealtorTool.Desktop.Services.Interfaces;

public interface IDealService
{
    /// <summary>
    /// Создать сделку на основе двух заявок
    /// </summary>
    public Task<Deal> CreateDealAsync(string buyApplicationId, string saleApplicationId, decimal finalPrice, string employeeId);
    
    /// <summary>
    /// Получить сделку по идентификатору
    /// </summary>
    public Task<Deal?> GetDealByIdAsync(string id);
    
    /// <summary>
    /// Обновить статус сделки
    /// </summary>
    public Task UpdateDealStatusAsync(string dealId, string statusId);
    
    /// <summary>
    /// Рассчитать комиссию по сделке
    /// </summary>
    public decimal CalculateCommission(decimal dealPrice, decimal commissionRate = 0.03m);
}