using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.ViewModels.Windows;

/// <summary>
/// VM Главного окна.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    private Employee CurrentAuth { get; set; }
    
    public ObservableCollection<PageViewModelBase> PaneItems { get; set; }
    
    [Reactive] public PageViewModelBase SelectedPageItem { get; set; }
    
    public ICommand OpenMyProfilePage { get; private set; }
    public ICommand OpenHomePage { get; private set; }
    public ICommand OpenCreatingApplicationPage { get; private set; }
    public ICommand OpenEmployeesPage { get; private set; }
    
    public bool CanOpenMyProfile { get; } = false;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainWindowViewModel(
        MyProfilePageViewModel myProfilePageViewModel,
        PersonProfilePageViewModel personProfilePageViewModel,
        HomePageViewModel homePageViewModel,
        CreatingApplicationPageViewModel creatingApplicationPageViewModel,
        EmployeesPageViewModel employeesPageViewModel)
    {
        PaneItems = 
        [
            myProfilePageViewModel, 
            personProfilePageViewModel, 
            homePageViewModel,
            creatingApplicationPageViewModel,
            employeesPageViewModel
        ];
        SelectedPageItem = PaneItems[2];
        
        InitialButtons();
        GetDataFromMessageBus();
    }

    private void InitialButtons()
    {
        OpenMyProfilePage = ReactiveCommand.Create(OpenMyProfilePageImpl);
        OpenHomePage = ReactiveCommand.Create(OpenHomePageImpl);
        OpenCreatingApplicationPage = ReactiveCommand.Create(OpenCreatingApplicationPageImpl);
        OpenEmployeesPage = ReactiveCommand.Create(OpenEmployeesPageImpl);
    }

    private void OpenHomePageImpl() => SelectedPageItem = PaneItems[2];
    private void OpenMyProfilePageImpl() => SelectedPageItem = PaneItems[0];
    private void OpenCreatingApplicationPageImpl() => SelectedPageItem = PaneItems[3];

    private void OpenEmployeesPageImpl() => SelectedPageItem = PaneItems[4];

    private void GetDataFromMessageBus()
    {
        MessageBus.Current
            .Listen<Employee>("CurrentAuth")
            .Subscribe(x => 
            {
                CurrentAuth = x;
            });
    }
}