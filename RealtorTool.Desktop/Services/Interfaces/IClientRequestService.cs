using System.Collections.Generic;
using System.Threading.Tasks;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.Enums;

namespace RealtorTool.Desktop.Services.Interfaces;

public interface IClientRequestService
{
    /// <summary>
    /// Создать заявку на операцию с недвижимостью
    /// </summary>
    public Task<ClientRequest> CreateClientRequestAsync(ClientRequest ClientRequest, Realty? realty = null);
    
    /// <summary>
    /// Найти подходящие варианты недвижимости для заявки
    /// </summary>
    public Task<List<Listing>> FindMatchingListingsAsync(ClientRequest ClientRequest);
    
    /// <summary>
    /// Обновить статус заявки
    /// </summary>
    public Task UpdateClientRequestStatusAsync(string ClientRequestId, ApplicationStatus status);
    
    /// <summary>
    /// Получить заявки по клиенту
    /// </summary>
    public Task<List<ClientRequest>> GetClientRequestsByClientAsync(string clientId);
    
    /// <summary>
    /// Получить заявки по сотруднику
    /// </summary>
    public Task<List<ClientRequest>> GetClientRequestsByEmployeeAsync(string employeeId);
}