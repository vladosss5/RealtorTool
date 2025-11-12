using System.Threading.Tasks;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.Enums;

namespace RealtorTool.Desktop.Services.Interfaces;

public interface IAccountingService
{
    public Task<Employee?> LoginAsync(string? login, string? password);

    public (string Hash, string Salt) HashPassword(string password);
    
    public Employee? CurrentUser { get; }
    
    public UserRole? CurrentRole { get; }
   
    public bool IsAuthenticated { get; }
    
    public bool HasRole(UserRole role);
    
    public bool HasAnyRole(params UserRole[] roles);
    
    public void SetCurrentUser(Employee employee);
    
    public void Logout();
}