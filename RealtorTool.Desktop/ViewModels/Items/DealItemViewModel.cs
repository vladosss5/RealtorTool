using System.Linq;
using RealtorTool.Core.DbEntities;

namespace RealtorTool.Desktop.ViewModels.Items;

public class DealItemViewModel : ViewModelBase
{
    public Deal Deal { get; set; }

    public DealItemViewModel(Deal deal)
    {
        Deal = deal;
    }

    // Главное изображение из связанного объекта недвижимости
    public byte[]? MainImage => Deal.Listing?.Realty?.Photos
        .OrderBy(p => p.SortOrder)
        .FirstOrDefault()?.FileData;

    public bool HasImage => MainImage != null;

    // Форматированная дата сделки
    public string FormattedDealDate => Deal.DealDate.ToString("dd.MM.yyyy");

    // Форматированная финальная цена
    public string FormattedFinalPrice => $"{Deal.FinalPrice:N0}";

    // Форматированная комиссия
    public string FormattedCommission => $"{Deal.Commission:N0}";

    // Основной покупатель
    public string BuyerName => Deal.Buyer != null 
        ? $"{Deal.Buyer.LastName} {Deal.Buyer.FirstName} {Deal.Buyer.MiddleName}"
        : "Не указан";

    // Ответственный сотрудник
    public string EmployeeName => Deal.Employee != null
        ? $"{Deal.Employee.LastName} {Deal.Employee.FirstName}"
        : "Не назначен";

    // Название объекта недвижимости
    public string RealtyName => Deal.Listing?.Realty?.Name ?? "Объект не указан";

    // Адрес объекта
    public string Address => Deal.Listing?.Realty?.Address != null
        ? $"{Deal.Listing.Realty.Address.City}, {Deal.Listing.Realty.Address.Street}"
        : "Адрес не указан";

    // Количество участников сделки
    public int ParticipantsCount => Deal.Participants?.Count ?? 0;

    // Статус сделки
    public string Status => Deal.Status?.Value ?? "Не указан";

    // Тип сделки
    public string DealType => Deal.DealType?.Value ?? "Не указан";
}