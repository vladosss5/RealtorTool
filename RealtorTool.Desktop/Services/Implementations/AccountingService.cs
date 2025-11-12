using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.Services.Implementations;

public class AccountingService : IAccountingService
{
    private readonly DataContext _context;
    
    public Employee? CurrentUser { get; private set; }
    public UserRole? CurrentRole { get; private set; }
    public bool IsAuthenticated => CurrentUser != null;
    
    public AccountingService(DataContext context)
    {
        _context = context;
    }
    
    public bool HasRole(UserRole role)
    {
        return CurrentRole == role;
    }

    public bool HasAnyRole(params UserRole[] roles)
    {
        return CurrentRole.HasValue && roles.Contains(CurrentRole.Value);
    }
    
    public void SetCurrentUser(Employee employee)
    {
        CurrentUser = employee;
        
        // Конвертируем строковую роль в enum
        if (employee.Role?.Value != null)
        {
            CurrentRole = employee.Role.Value.ToLower() switch
            {
                "системный администратор" => UserRole.SystemAdministrator,
                "система" => UserRole.System,
                "администратор" => UserRole.Administrator,
                "риэлтор" => UserRole.Realtor,
                _ => null
            };
        }
    }

    public void Logout()
    {
        CurrentUser = null;
        CurrentRole = null;
    }

    /// <inheritdoc />
    public async Task<Employee?> LoginAsync(string? login, string? password)
    {
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            return null;

        var employee = await _context.Employees
            .FirstOrDefaultAsync(a => a.Login == login);

        if (employee == null)
            return null;
        
        if (!VerifyPassword(password, employee.PasswordHash, employee.Salt))
            return null;

        return employee;
    }

    public (string Hash, string Salt) HashPassword(string password)
    {
        byte[] saltBytes = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(saltBytes);
        }
        string salt = Convert.ToBase64String(saltBytes);

        string hash = ComputeHash(password, salt);
        return (hash, salt);
    }

    private static bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        string computedHash = ComputeHash(password, storedSalt);
        return storedHash == computedHash;
    }

    private static string ComputeHash(string password, string salt)
    {
        using var pbkdf2 = new Rfc2898DeriveBytes(
            password,
            Convert.FromBase64String(salt),
            10000, 
            HashAlgorithmName.SHA256);

        byte[] hashBytes = pbkdf2.GetBytes(32);
        return Convert.ToBase64String(hashBytes);
    }
}