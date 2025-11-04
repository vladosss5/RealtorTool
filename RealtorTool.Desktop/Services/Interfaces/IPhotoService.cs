using System.Collections.Generic;
using System.Threading.Tasks;
using Avalonia.Controls;
using RealtorTool.Core.Enums;
using RealtorTool.Desktop.DTOs;

namespace RealtorTool.Desktop.Services.Interfaces;

/// <summary>
/// Сервис для работы с изображениями и фотографиями
/// </summary>
public interface IPhotoService
{
    /// <summary>
    /// Выбрать изображения через диалоговое окно
    /// </summary>
    Task<List<UploadedPhoto>> SelectImagesAsync(Window ownerWindow = null);
    
    /// <summary>
    /// Загрузить изображение из файла
    /// </summary>
    Task<UploadedPhoto?> LoadImageAsync(string filePath);
    
    /// <summary>
    /// Удалить превью изображения (освободить ресурсы)
    /// </summary>
    void DisposeImagePreview(UploadedPhoto photo);
    
    /// <summary>
    /// Сохранить фотографии в базу данных для указанной сущности
    /// </summary>
    Task<string[]?> SavePhotosToDatabaseAsync(List<UploadedPhoto> uploadedPhotos, string entityId, EntityTypeForPhoto entityType);
    
    /// <summary>
    /// Загрузить фотографии из базы данных для указанной сущности
    /// </summary>
    Task<List<UploadedPhoto>> LoadPhotosFromDatabaseAsync(string entityId, EntityTypeForPhoto entityType);
    
    /// <summary>
    /// Удалить фотографию из базы данных
    /// </summary>
    Task DeletePhotoAsync(string photoId);
    
    /// <summary>
    /// Форматирует размер файла в читаемый вид
    /// </summary>
    string FormatFileSize(long bytes);
}