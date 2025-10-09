using RealtorTool.Core.DbEntities;

namespace RealtorTool.Services.Interfaces;

public interface IAccountingService
{
    public Task<Employee?> LoginAsync(string? login, string? password);

    public (string Hash, string Salt) HashPassword(string password);
}