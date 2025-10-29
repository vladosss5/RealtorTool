using System.Collections.Generic;
using System.Threading.Tasks;
using RealtorTool.Core.DbEntities.Views;

namespace RealtorTool.Desktop.Services.Interfaces;

public interface IMatchingService
{
    Task<List<PotentialMatch>> FindPotentialMatchesAsync(string buyRequestId);
    Task<List<PotentialMatch>> FindPotentialMatchesForSellRequestAsync(string sellRequestId);
    Task<List<PotentialMatch>> FindBestMatchesAsync(string buyRequestId, int limit = 10);
    Task<PotentialMatch?> FindBestMatchAsync(string buyRequestId);
    Task<bool> CreateMatchAsync(string buyRequestId, string sellRequestId);
}