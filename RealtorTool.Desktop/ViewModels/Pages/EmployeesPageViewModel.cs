using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MsBox.Avalonia;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.Enums;
using RealtorTool.Data.Context;
using RealtorTool.Desktop.DTOs;
using RealtorTool.Desktop.Services.Interfaces;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class EmployeesPageViewModel : AccountViewModelBase
{
    private readonly DataContext _context;
    private readonly IAccountingService _accountingService;
    private readonly IPhotoService _photoService;
    private readonly IServiceProvider _serviceProvider;
    private readonly INavigationService _navigationService;
    
    private Employee? _selectedEmployee;
    
    [Reactive] public string FName { get; set; }
    [Reactive] public string SName { get; set; }
    [Reactive] public string LName { get; set; }
    [Reactive] public string Login { get; set; }
    
    [Reactive] public ObservableCollection<Employee> Employees { get; set; } = new();
    [Reactive] public List<DictionaryValue> Roles { get; set; } = new();
    [Reactive] public DictionaryValue SelectedRole { get; set; } = new();
    
    // Заменяем коллекцию на одно свойство для фотографии
    [Reactive] public UploadedPhoto? CurrentPhoto { get; set; }
    [Reactive] public string PhotosSummary { get; set; } = "Фотография не добавлена";
    
    private const string Password = "qweasd123";
    
    public Employee SelectedEmployee
    {
        get => _selectedEmployee;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedEmployee, value);
            OpenEmployeeDetailAsync(_selectedEmployee.Id);
            _selectedEmployee = null;
        }
    }

    public bool HasPhoto => CurrentPhoto != null;
    
    public ICommand CreateEmployeeCommand { get; }
    public ReactiveCommand<Unit, Unit> SelectImageCommand { get; }
    public ReactiveCommand<UploadedPhoto?, Unit> RemovePhotoCommand { get; }

    public EmployeesPageViewModel(
        DataContext context,
        IPhotoService photoService, 
        IServiceProvider serviceProvider, 
        INavigationService navigationService, 
        IAccountingService accountingService) 
        : base(accountingService)
    {
        _context = context;
        _accountingService = accountingService;
        _photoService = photoService;
        _serviceProvider = serviceProvider;
        _navigationService = navigationService;

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
                .Include(x => x.Role)
                .Where(x => !x.IsDeleted && x.RoleId != "system_role")
                .ToListAsync();
                
            Employees.Clear();
            Employees.AddRange(employees);

            if (!Roles.Any())
            {
                var roles = await _context.DictionaryValues
                    .Where(x => 
                        x.DictionaryId == "employee_role" && 
                        x.Id != "system_role")
                    .ToListAsync();
                
                Roles.AddRange(roles);
            }
        }
        catch (System.Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Не удалось загрузить список сотрудников: {ex.Message}")
                .ShowAsync();
        }
    }

    private async Task CreateEmployeeAsync()
    {
        if (string.IsNullOrWhiteSpace(FName) || string.IsNullOrWhiteSpace(LName))
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Заполните имя и фамилию сотрудника")
                .ShowAsync();
            return;
        }

        if (string.IsNullOrWhiteSpace(Login) || string.IsNullOrWhiteSpace(Password))
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Заполните логин и пароль")
                .ShowAsync();
            return;
        }

        if (string.IsNullOrWhiteSpace(SelectedRole.Value))
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", "Выберите роль")
                .ShowAsync();
            return;
        }

        try
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var newEmployee = new Employee();
            
            var passwordHashAndSalt = _accountingService.HashPassword(Password);
            
            newEmployee.Login = Login;
            newEmployee.FirstName = FName;
            newEmployee.LastName = LName;
            newEmployee.MiddleName = SName;
            newEmployee.PasswordHash = passwordHashAndSalt.Hash;
            newEmployee.Salt = passwordHashAndSalt.Salt;
            newEmployee.RoleId = SelectedRole.Id;
            
            _context.Add(newEmployee);
            await _context.SaveChangesAsync();

            if (CurrentPhoto != null)
            {
                var photoIds = await _photoService.SavePhotosToDatabaseAsync(
                    new List<UploadedPhoto> { CurrentPhoto }, 
                    newEmployee.Id, 
                    EntityTypeForPhoto.Employee);

                newEmployee.PhotoId = photoIds![0];
            }

            _context.Attach(newEmployee);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            await MessageBoxManager
                .GetMessageBoxStandard("Успех", "Сотрудник успешно создан")
                .ShowAsync();

            ClearForm();
            LoadData();
        }
        catch (DbUpdateException dbEx)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка базы данных", dbEx.InnerException?.Message ?? dbEx.Message)
                .ShowAsync();
        }
        catch (System.Exception ex)
        {
            await MessageBoxManager
                .GetMessageBoxStandard("Ошибка", $"Не удалось создать сотрудника: {ex.Message}")
                .ShowAsync();
        }
    }

    private async Task SelectImageAsync()
    {
        var newPhotos = await _photoService.SelectImagesAsync();
        if (newPhotos.Any())
        {
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
        FName = "";
        SName = "";
        LName = "";
        Login = "";
        CurrentPhoto = null;
        UpdatePhotosSummary();
        this.RaisePropertyChanged(nameof(HasPhoto));
    }
    
    private void OpenEmployeeDetailAsync(string selectedEmployeeId)
    {
        var detailVm = _serviceProvider.GetRequiredService<EmployeeDetailViewModel>();
        
        if (detailVm is IParameterReceiver parameterReceiver)
            parameterReceiver.ReceiveParameter(selectedEmployeeId);
        
        MessageBus.Current.SendMessage(detailVm, "NavigateToPage");

        _navigationService.NavigateTo(detailVm);
        
        MessageBus.Current.SendMessage(selectedEmployeeId, "SelectedEmployeeId");
    }
}