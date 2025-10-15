using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.DTOs;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

public class SellApplicationPageViewModel : ViewModelBase
{
    /// Сервисы.
    private readonly DataContext _context;
    private readonly IWindowService _windowService;

    // Поля.
    [Reactive] public Address NewAddress { get; set; } = new();
    [Reactive] public Listing NewListing { get; set; } = new();
    [Reactive] public Client NewClient { get; set; } = new();
    [Reactive] public Area NewArea { get; set; } = new();
    [Reactive] public Apartment NewApartament { get; set; } = new();
    [Reactive] public ObservableCollection<UploadedPhoto> UploadedPhotos { get; set; } = new();
    [Reactive] public string PhotosSummary { get; set; } = "Фотографии не добавлены";

    public bool HasPhotos => UploadedPhotos.Any();

    // Коллекции.
    public ObservableCollection<DictionaryValue> RenovationTypes { get; set; } = new();
    public ObservableCollection<DictionaryValue> BathroomTypes { get; set; } = new();
    
    public RealtyType CurrentRealtyType { get; private set; }
    
    private int _selectedPropertyTypeIndex;
    public int SelectedPropertyTypeIndex
    {
        get => _selectedPropertyTypeIndex;
        set 
        {
            this.RaiseAndSetIfChanged(ref _selectedPropertyTypeIndex, value);
            CurrentRealtyType = SelectedPropertyTypeIndex switch
            {
                0 => RealtyType.Apartment,
                1 => RealtyType.Area,
                2 => RealtyType.PrivateHouse,
                _ => CurrentRealtyType
            };
        }
    }
    
    // Кнопки.
    public ICommand CreateSellRequest { get; }
    public ReactiveCommand<Unit, Unit> SelectImagesCommand { get; }
    public ReactiveCommand<UploadedPhoto, Unit> RemovePhotoCommand { get; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="windowService"></param>
    public SellApplicationPageViewModel(DataContext context, IWindowService windowService)
    {
        _context = context;
        _windowService = windowService;
        CreateSellRequest = ReactiveCommand.CreateFromTask(CreateSellRequestAsync);
        
        SelectImagesCommand = ReactiveCommand.CreateFromTask(SelectImagesAsync);
        RemovePhotoCommand = ReactiveCommand.Create<UploadedPhoto>(RemovePhoto);

        _ = InitializeAsync();
    }
    
    private async Task InitializeAsync()
    {
        var renovationTypesList = await _context.DictionaryValues
            .Where(x => x.DictionaryId == "renovation_type")
            .ToListAsync();
        RenovationTypes.AddRange(renovationTypesList);
        
        var bathroomTypes = await _context.DictionaryValues
            .Where(x => x.DictionaryId == "bathroom_type")
            .ToListAsync();
        BathroomTypes.AddRange(bathroomTypes);   
    }

    private async Task CreateSellRequestAsync()
    {
        try
        {
            await AddClientDataToContextAsync();
            await AddAddressDataToContextAsync();

            switch (CurrentRealtyType)
            {
                case RealtyType.Area:
                    await AddAreaDataToContextAsync();
                    break;
                case RealtyType.Apartment:
                    await AddApartmentDataToContextAsync();
                    NewListing.Realty = NewApartament;
                    break;
                case RealtyType.PrivateHouse:
                    await AddPrivateHouseDataToContextAsync();
                    break;
            }
            
            NewListing.Owner = NewClient;
            NewListing.CurrencyId = "currency_rub";
            NewListing.ListingTypeId = "listing_sale";
            NewListing.StatusId = "listing_active";

            _context.Add(NewListing);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", e.Message)
                .ShowAsync();
        }
    }

    private async Task AddAddressDataToContextAsync()
    {
        await _context.AddAsync(NewAddress);
    }

    private async Task AddPrivateHouseDataToContextAsync()
    {
        throw new NotImplementedException();
    }

    private async Task AddApartmentDataToContextAsync()
    {
        NewApartament.RenovationType = RenovationTypes.FirstOrDefault(x => x.IsSelected);
        NewApartament.BathroomType = BathroomTypes.FirstOrDefault(x => x.IsSelected);
        NewApartament.Address = NewAddress;
        await _context.AddAsync(NewApartament);
    }

    private async Task AddAreaDataToContextAsync()
    {
        throw new NotImplementedException();
    }

    private async Task AddClientDataToContextAsync()
    {
        await _context.AddAsync(NewClient);
    }
    
    private async Task SelectImagesAsync()
    {
        try
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите фотографии объекта недвижимости",
                AllowMultiple = true,
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter
                    {
                        Name = "Изображения",
                        Extensions = new List<string> { "jpg", "jpeg", "png", "bmp", "gif" }
                    }
                }
            };

            // Получаем окно
            var ownerWindow = GetDialogOwnerWindow();
        
            if (ownerWindow == null)
            {
                Console.WriteLine("Не удалось определить окно для диалога");
                return;
            }

            var result = await dialog.ShowAsync(ownerWindow);
        
            if (result != null && result.Any())
            {
                await ProcessSelectedFiles(result);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при выборе файлов: {ex.Message}");
        }
    }
    
    private Window? GetDialogOwnerWindow()
    {
        // Пробуем разные способы по порядку
    
        // 1. Через WindowService
        var window = _windowService.GetMainWindow();
        if (window != null) return window;

        // 2. Через ApplicationLifetime
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            window = desktopLifetime.MainWindow ?? desktopLifetime.Windows.FirstOrDefault();
            if (window != null) return window;
        }

        // 3. Через TopLevel
        window = TopLevel.GetTopLevel(null) as Window;
        if (window != null) return window;

        // 4. Если ничего не сработало, пробуем создать временное окно
        Console.WriteLine("Не удалось найти существующее окно, будет создано временное");
        return CreateTemporaryWindow();
    }

    private Window? CreateTemporaryWindow()
    {
        try
        {
            // Создаем временное невидимое окно для диалога
            return new Window
            {
                Width = 0,
                Height = 0,
                WindowStartupLocation = WindowStartupLocation.CenterScreen,
                ShowInTaskbar = false
            };
        }
        catch
        {
            return null;
        }
    }

    private async Task ProcessSelectedFiles(string[] filePaths)
    {
        foreach (var filePath in filePaths)
        {
            // Проверяем что файл является изображением
            if (IsImageFile(filePath))
            {
                await LoadAndAddImage(filePath);
            }
        }
        
        UpdatePhotosSummary();
    }

    private bool IsImageFile(string filePath)
    {
        var extensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        var extension = Path.GetExtension(filePath).ToLower();
        return extensions.Contains(extension);
    }

    private async Task LoadAndAddImage(string filePath)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);
            
            // Проверяем размер файла (максимум 10MB)
            if (fileInfo.Length > 10 * 1024 * 1024)
            {
                // Можно показать сообщение о слишком большом файле
                return;
            }

            // Читаем файл
            var fileData = await File.ReadAllBytesAsync(filePath);
            
            // Создаем превью
            using var stream = new MemoryStream(fileData);
            var bitmap = new Bitmap(stream);
            
            // Создаем объект загруженного фото
            var uploadedPhoto = new UploadedPhoto
            {
                FileName = Path.GetFileName(filePath),
                FileSize = fileInfo.Length,
                FileSizeFormatted = FormatFileSize(fileInfo.Length),
                FileData = fileData,
                Preview = bitmap,
                ContentType = GetContentType(filePath)
            };

            UploadedPhotos.Add(uploadedPhoto);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки изображения {filePath}: {ex.Message}");
        }
    }

    private void RemovePhoto(UploadedPhoto photo)
    {
        UploadedPhotos.Remove(photo);
        photo.Preview?.Dispose();
        UpdatePhotosSummary();
    }

    private void UpdatePhotosSummary()
    {
        if (!UploadedPhotos.Any())
        {
            PhotosSummary = "Фотографии не добавлены";
            return;
        }

        var totalSize = UploadedPhotos.Sum(p => p.FileSize);
        PhotosSummary = $"{UploadedPhotos.Count} фото • {FormatFileSize(totalSize)}";
    }

    private string FormatFileSize(long bytes)
    {
        string[] suffixes = { "B", "KB", "MB", "GB" };
        int counter = 0;
        decimal number = bytes;
        while (Math.Round(number / 1024) >= 1)
        {
            number /= 1024;
            counter++;
        }
        return $"{number:n1} {suffixes[counter]}";
    }

    private string GetContentType(string filePath)
    {
        return Path.GetExtension(filePath).ToLower() switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".bmp" => "image/bmp",
            ".gif" => "image/gif",
            _ => "application/octet-stream"
        };
    }
}