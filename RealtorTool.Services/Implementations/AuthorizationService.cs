using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbModels.PersonModels;
using RealtorTool.Data;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Services.Implementations;

public class AuthorizationService : IAuthorizationService
{
    private readonly DataContext _context;
    
    public AuthorizationService(DataContext context)
    {
        _context = context;
    }


    /// <inheritdoc />
    public async Task<Person?> LoginAsync(string? login, string? password)
    {
        if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            return null;
        
        var authData = await _context.AuthorizationData
            .Include(a => a.Person)
            .FirstOrDefaultAsync(a => a.Login == login);

        if (authData == null)
            return null;
        
        if (!VerifyPassword(password, authData.PasswordHash, authData.Salt))
            return null;

        return authData.Person;
    }

    public static (string Hash, string Salt) HashPassword(string password)
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