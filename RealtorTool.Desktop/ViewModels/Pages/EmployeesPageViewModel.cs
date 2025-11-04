using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
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
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class EmployeesPageViewModel : PageViewModelBase
{
    private readonly DataContext _context;
    private readonly IAccountingService _accountingService;
    private readonly IPhotoService _photoService;
    
    [Reactive] public ObservableCollection<Employee> Employees { get; set; } = new();
    [Reactive] public Employee NewEmployee { get; set; } = new();
    [Reactive] public string Password { get; set; } = string.Empty;
    
    // Заменяем коллекцию на одно свойство для фотографии
    [Reactive] public UploadedPhoto? CurrentPhoto { get; set; }
    [Reactive] public string PhotosSummary { get; set; } = "Фотография не добавлена";
    
    public bool HasPhoto => CurrentPhoto != null;
    
    public ICommand CreateEmployeeCommand { get; }
    public ReactiveCommand<Unit, Unit> SelectImageCommand { get; }
    public ReactiveCommand<UploadedPhoto?, Unit> RemovePhotoCommand { get; }

    public EmployeesPageViewModel(
        DataContext context, 
        IAccountingService accountingService,
        IPhotoService photoService)
    {
        _context = context;
        _accountingService = accountingService;
        _photoService = photoService;

        LoadData();
        
        CreateEmployeeCommand = ReactiveCommand.CreateFromTask(CreateEmployeeAsync);
        SelectImageCommand = ReactiveCommand.CreateFromTask(SelectImageAsync);
        RemovePhotoCommand = ReactiveCommand.Create<UploadedPhoto?>(RemovePhoto);
    }

    private async void LoadData()
    {
        try
        {
            var employees = await _context.Employees
                .Include(x => x.Photo)
                .ToListAsync();
                
            Employees.Clear();
            Employees.AddRange(employees);
        }
        catch (System.Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Не удалось загрузить список сотрудников: {ex.Message}", ButtonEnum.Ok)
                .ShowAsync();
        }
    }

    private async Task CreateEmployeeAsync()
    {
        if (string.IsNullOrWhiteSpace(NewEmployee.FirstName) || string.IsNullOrWhiteSpace(NewEmployee.LastName))
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Заполните имя и фамилию сотрудника", ButtonEnum.Ok)
                .ShowAsync();
            return;
        }

        if (string.IsNullOrWhiteSpace(NewEmployee.Login) || string.IsNullOrWhiteSpace(Password))
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Заполните логин и пароль", ButtonEnum.Ok)
                .ShowAsync();
            return;
        }

        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var passwordHashAndSalt = _accountingService.HashPassword(Password);
            NewEmployee.PasswordHash = passwordHashAndSalt.Hash;
            NewEmployee.Salt = passwordHashAndSalt.Salt;
            
            await _context.Employees.AddAsync(NewEmployee);
            await _context.SaveChangesAsync();

            // Сохраняем фотографию если есть
            if (CurrentPhoto != null)
            {
                // Исправлено: создаем список вместо массива
                await _photoService.SavePhotosToDatabaseAsync(
                    new List<UploadedPhoto> { CurrentPhoto }, 
                    NewEmployee.Id, 
                    EntityTypeForPhoto.Employee);
            }

            await transaction.CommitAsync();

            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Сотрудник успешно создан", ButtonEnum.Ok)
                .ShowAsync();

            ClearForm();
            LoadData();
        }
        catch (DbUpdateException dbEx)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка базы данных", dbEx.InnerException?.Message ?? dbEx.Message, ButtonEnum.Ok)
                .ShowAsync();
        }
        catch (System.Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Не удалось создать сотрудника: {ex.Message}", ButtonEnum.Ok)
                .ShowAsync();
        }
    }

    private async Task SelectImageAsync()
    {
        var newPhotos = await _photoService.SelectImagesAsync();
        if (newPhotos.Any())
        {
            // Для сотрудника разрешаем только одну фотографию
            CurrentPhoto = newPhotos.First();
            UpdatePhotosSummary();
            this.RaisePropertyChanged(nameof(HasPhoto));
        }
    }

    private void RemovePhoto(UploadedPhoto? photo)
    {
        if (photo != null)
        {
            _photoService.DisposeImagePreview(photo);
        }
        CurrentPhoto = null;
        UpdatePhotosSummary();
        this.RaisePropertyChanged(nameof(HasPhoto));
    }

    private void UpdatePhotosSummary()
    {
        if (CurrentPhoto == null)
        {
            PhotosSummary = "Фотография не добавлена";
            return;
        }

        PhotosSummary = $"{CurrentPhoto.FileName} • {_photoService.FormatFileSize(CurrentPhoto.FileSize)}";
    }

    private void ClearForm()
    {
        NewEmployee = new Employee();
        Password = string.Empty;
        CurrentPhoto = null;
        UpdatePhotosSummary();
        this.RaisePropertyChanged(nameof(HasPhoto));
    }
}