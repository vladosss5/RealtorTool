using RealtorTool.Core.DbModels.PersonModels;

namespace RealtorTool.Services.Interfaces;

public interface IAuthorizationService
{
    public Task<Person?> LoginAsync(string? login, string? password);
}