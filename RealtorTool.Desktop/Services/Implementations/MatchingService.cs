using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.DbEntities.Views;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.Services.Implementations;

public class MatchingService : IMatchingService
{
    private readonly DataContext _context;

    public MatchingService(DataContext context)
    {
        _context = context;
    }

    public async Task<List<PotentialMatch>> FindPotentialMatchesAsync(string buyRequestId)
    {
        var matches = await _context.PotentialMatches
            .Where(x => x.BuyRequestId == buyRequestId)
            .AsNoTracking()
            .ToListAsync();

        return matches.OrderByDescending(m => m.MatchScore).ToList();
    }

    public async Task<List<PotentialMatch>> FindPotentialMatchesForSellRequestAsync(string sellRequestId)
    {
        var matches = await _context.PotentialMatches
            .Where(x => x.SellRequestId == sellRequestId)
            .AsNoTracking()
            .ToListAsync();

        return matches.OrderByDescending(m => m.MatchScore).ToList();
    }

    public async Task<List<PotentialMatch>> FindBestMatchesAsync(string buyRequestId, int limit = 10)
    {
        var allMatches = await FindPotentialMatchesAsync(buyRequestId);
        return allMatches
            .Where(m => m.IsGoodMatch)
            .Take(limit)
            .ToList();
    }

    public async Task<PotentialMatch?> FindBestMatchAsync(string buyRequestId)
    {
        var matches = await FindPotentialMatchesAsync(buyRequestId);
        return matches.FirstOrDefault(m => m.IsPerfectMatch) ?? matches.FirstOrDefault();
    }

    // Метод для массового матчинга всех заявок
    public async Task<Dictionary<string, List<PotentialMatch>>> FindAllPotentialMatchesAsync()
    {
        var allMatches = await _context.PotentialMatches
            .FromSqlRaw("SELECT * FROM potential_matches")
            .AsNoTracking()
            .ToListAsync();

        return allMatches
            .GroupBy(m => m.BuyRequestId)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(m => m.MatchScore).ToList());
    }
    
    public async Task<bool> CreateMatchAsync(string buyRequestId, string sellRequestId)
{
    using var transaction = await _context.Database.BeginTransactionAsync();
    
    try
    {
        // Находим заявки с включением связанных данных
        var buyRequest = await _context.ClientRequests
            .Include(cr => cr.Client)
            .Include(cr => cr.Employee)
            .FirstOrDefaultAsync(cr => cr.Id == buyRequestId);

        var sellRequest = await _context.ClientRequests
            .Include(cr => cr.Listing)
                .ThenInclude(l => l.Realty)
            .Include(cr => cr.Client)
            .FirstOrDefaultAsync(cr => cr.Id == sellRequestId);

        if (buyRequest == null || sellRequest == null)
        {
            throw new ArgumentException("Одна из заявок не найдена");
        }

        if (sellRequest.Listing == null)
        {
            throw new InvalidOperationException("У заявки на продажу отсутствует объявление");
        }

        // Обновляем статусы заявок
        buyRequest.Status = ApplicationStatus.Matched;
        sellRequest.Status = ApplicationStatus.Matched;
        buyRequest.MatchedRequestId = sellRequestId;
        sellRequest.MatchedRequestId = buyRequestId;

        // Определяем тип сделки
        var dealTypeId = GetDealTypeId(buyRequest.Type);

        // Создаем сделку
        var deal = new Deal
        {
            ListingId = sellRequest.ListingId,
            BuyerId = buyRequest.ClientId,
            EmployeeId = buyRequest.EmployeeId,
            FinalPrice = sellRequest.Listing.Price,
            Commission = CalculateCommission(sellRequest.Listing.Price, buyRequest.Type),
            DealDate = DateTime.UtcNow,
            DealTypeId = dealTypeId,
            StatusId = "deal_in_progress" // предполагаем, что такой ID есть в словаре
        };

        await _context.Deals.AddAsync(deal);
        await _context.SaveChangesAsync(); // Сохраняем, чтобы получить ID сделки

        // Создаем участников сделки
        var participants = new List<DealParticipant>
        {
            new DealParticipant
            {
                DealId = deal.Id,
                ClientRequestId = buyRequest.Id,
                Role = ParticipantRole.Buyer
            },
            new DealParticipant
            {
                DealId = deal.Id,
                ClientRequestId = sellRequest.Id,
                Role = ParticipantRole.Seller
            }
        };

        await _context.DealParticipants.AddRangeAsync(participants);
        await _context.SaveChangesAsync();

        // Фиксируем транзакцию
        await transaction.CommitAsync();

        return true;
    }
    catch (Exception ex)
    {
        await transaction.RollbackAsync();
        
        // Логируем ошибку
        Console.WriteLine($"Ошибка при создании сделки: {ex.Message}");
        Debug.WriteLine($"Ошибка при создании сделки: {ex}");
        
        return false;
    }
}

// Вспомогательные методы
private string GetDealTypeId(ApplicationType requestType)
{
    return requestType switch
    {
        ApplicationType.Purchase or ApplicationType.Sale => "deal_sale",
        ApplicationType.Rent or ApplicationType.RentOut => "deal_rent",
        _ => "deal_sale"
    };
}

private decimal CalculateCommission(decimal price, ApplicationType requestType)
{
    // Разная комиссия для продажи и аренды
    var commissionRate = requestType switch
    {
        ApplicationType.Purchase or ApplicationType.Sale => 0.03m, // 3%
        ApplicationType.Rent or ApplicationType.RentOut => 0.05m,  // 5%
        _ => 0.03m
    };
    
    return price * commissionRate;
}
}