using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.DTOs;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.Services.Implementations;

public class PhotoService : IPhotoService
{
    private readonly DataContext _context;
    
    public PhotoService(
        DataContext context)
    {
        _context = context;
    }
    
    public async Task<List<UploadedPhoto>> SelectImagesAsync(Window ownerWindow = null)
    {
        var uploadedPhotos = new List<UploadedPhoto>();
    
        try
        {
            var dialog = new OpenFileDialog
            {
                Title = "Выберите фотографии",
                AllowMultiple = true,
                Filters = new List<FileDialogFilter>
                {
                    new()
                    {
                        Name = "Изображения",
                        Extensions = new List<string> { "jpg", "jpeg", "png", "bmp", "gif" }
                    }
                }
            };

            // Если окно не передано, пытаемся найти его
            ownerWindow ??= GetDialogOwnerWindow();
        
            if (ownerWindow == null)
            {
                Console.WriteLine("Не удалось определить окно для диалога");
                return uploadedPhotos;
            }

            var result = await dialog.ShowAsync(ownerWindow);
            if (result != null && result.Any())
            {
                // Обработка выбранных файлов
                foreach (var filePath in result)
                {
                    if (IsImageFile(filePath))
                    {
                        var uploadedPhoto = await LoadImageAsync(filePath);
                        if (uploadedPhoto != null)
                        {
                            uploadedPhotos.Add(uploadedPhoto);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при выборе файлов: {ex.Message}");
        }

        return uploadedPhotos;
    }

    private Window? GetDialogOwnerWindow()
    {
        try
        {
            if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Возвращаем активное окно или главное окно
                return desktop.Windows.FirstOrDefault(w => w.IsActive) ?? desktop.MainWindow;
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при получении окна: {ex.Message}");
            return null;
        }
    }

    public async Task<UploadedPhoto?> LoadImageAsync(string filePath)
    {
        try
        {
            var fileInfo = new FileInfo(filePath);

            // Проверяем размер файла (максимум 10MB)
            if (fileInfo.Length > 10 * 1024 * 1024)
            {
                Console.WriteLine($"Файл слишком большой: {filePath}");
                return null;
            }

            // Читаем файл
            var fileData = await File.ReadAllBytesAsync(filePath);

            // Создаем превью
            using var stream = new MemoryStream(fileData);
            var bitmap = new Bitmap(stream);

            // Создаем объект загруженного фото
            return new UploadedPhoto
            {
                FileName = Path.GetFileName(filePath),
                FilePath = filePath,
                FileSize = fileInfo.Length,
                FileSizeFormatted = FormatFileSize(fileInfo.Length),
                FileData = fileData,
                Preview = bitmap,
                ContentType = GetContentType(filePath)
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка загрузки изображения {filePath}: {ex.Message}");
            return null;
        }
    }

    public void DisposeImagePreview(UploadedPhoto photo)
    {
        photo.Preview?.Dispose();
    }

    public async Task<string[]?> SavePhotosToDatabaseAsync(List<UploadedPhoto> uploadedPhotos, string entityId, EntityTypeForPhoto entityType)
    {
        if (uploadedPhotos == null || !uploadedPhotos.Any()) 
            return null;

        var photos = uploadedPhotos.Select((uploaded, index) => new Photo
        {
            Id = Guid.NewGuid().ToString(),
            EntityType = entityType,
            EntityId = entityId,
            FileName = Path.GetFileName(uploaded.FileName),
            ContentType = uploaded.ContentType,
            FileData = uploaded.FileData ?? throw new ArgumentException("File data cannot be null"),
            SortOrder = index,
            IsMain = index == 0,
            CreatedDate = DateTime.UtcNow
        }).ToList();

        await _context.Photos.AddRangeAsync(photos);
        await _context.SaveChangesAsync();

        return photos.Select(x => x.Id).ToArray();
    }

    public async Task<List<UploadedPhoto>> LoadPhotosFromDatabaseAsync(string entityId, EntityTypeForPhoto entityType)
    {
        var photos = await _context.Photos
            .Where(p => p.EntityId == entityId && p.EntityType == entityType)
            .OrderBy(p => p.SortOrder)
            .ToListAsync();

        var uploadedPhotos = new List<UploadedPhoto>();

        foreach (var photo in photos)
        {
            try
            {
                using var stream = new MemoryStream(photo.FileData);
                var bitmap = new Bitmap(stream);

                uploadedPhotos.Add(new UploadedPhoto
                {
                    Id = photo.Id,
                    FileName = photo.FileName,
                    FileSize = photo.FileData.Length,
                    FileSizeFormatted = FormatFileSize(photo.FileData.Length),
                    FileData = photo.FileData,
                    Preview = bitmap,
                    ContentType = photo.ContentType,
                    IsMain = photo.IsMain
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки фото {photo.FileName}: {ex.Message}");
            }
        }

        return uploadedPhotos;
    }

    public async Task DeletePhotoAsync(string photoId)
    {
        var photo = await _context.Photos.FindAsync(photoId);
        if (photo != null)
        {
            _context.Photos.Remove(photo);
            await _context.SaveChangesAsync();
        }
    }
    
    private async Task<bool> CheckEntityExistsAsync(string entityId, EntityTypeForPhoto entityType)
    {
        switch (entityType)
        {
            case EntityTypeForPhoto.Realty:
            {
                var realty = await _context.Realties.FindAsync(entityId);
                return realty != null;
            }
            case EntityTypeForPhoto.Employee:
            {
                var employee = await _context.Employees.FindAsync(entityId);
                return employee != null;
            }
            case EntityTypeForPhoto.Client:
            {
                var client = await _context.Clients.FindAsync(entityId);
                return client != null;
            }
            default:
                return false;
        }
    }
    
    private async Task RemoveExistingPhotosAsync(string entityId, EntityTypeForPhoto entityType)
    {
        var existingPhotos = await _context.Photos
            .Where(p => p.EntityId == entityId && p.EntityType == entityType)
            .ToListAsync();
            
        _context.Photos.RemoveRange(existingPhotos);
        await _context.SaveChangesAsync();
    }

    public string FormatFileSize(long bytes)
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

    private bool IsImageFile(string filePath)
    {
        var extensions = new[] { ".jpg", ".jpeg", ".png", ".bmp", ".gif" };
        var extension = Path.GetExtension(filePath).ToLower();
        return extensions.Contains(extension);
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