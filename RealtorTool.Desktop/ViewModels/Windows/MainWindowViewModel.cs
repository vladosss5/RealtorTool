using System;
using System.Collections.ObjectModel;
using System.Reactive.Linq;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.ViewModels.Windows;

/// <summary>
/// VM Главного окна.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;
    private readonly IWindowService _windowService;

    private Employee CurrentAuth { get; set; }

    public ObservableCollection<PageViewModelBase> PaneItems { get; set; }

    [Reactive] public PageViewModelBase SelectedPageItem { get; set; }

    public ICommand OpenMyProfilePage { get; private set; }
    public ICommand OpenHomePage { get; private set; }
    public ICommand OpenCreatingApplicationPage { get; private set; }
    public ICommand OpenEmployeesPage { get; private set; }
    public ICommand OpenApplicationsPage { get; private set; }
    public ICommand LogoutCommand { get; private set; }
    public ICommand OpenDealListPage { get; private set; }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainWindowViewModel(
        MyProfilePageViewModel myProfilePageViewModel,
        PersonProfilePageViewModel personProfilePageViewModel,
        HomePageViewModel homePageViewModel,
        CreatingApplicationPageViewModel creatingApplicationPageViewModel,
        EmployeesPageViewModel employeesPageViewModel,
        ApplicationsPageViewModel applicationsPageViewModel,
        DealsListPageViewModel dealsListPageViewModel,
        INavigationService navigationService, 
        IWindowService windowService)
    {
        _navigationService = navigationService;
        _windowService = windowService;

        PaneItems =
        [
            myProfilePageViewModel,
            personProfilePageViewModel,
            homePageViewModel,
            creatingApplicationPageViewModel,
            employeesPageViewModel,
            applicationsPageViewModel,
            dealsListPageViewModel
        ];
        SelectedPageItem = PaneItems[2];
        
        _navigationService.OnNavigationRequested += OnNavigationRequested;
        
        InitialButtons();
        GetDataFromMessageBus();
    }

    private void InitialButtons()
    {
        OpenMyProfilePage = ReactiveCommand.Create(OpenMyProfilePageImpl);
        OpenHomePage = ReactiveCommand.Create(OpenHomePageImpl);
        OpenCreatingApplicationPage = ReactiveCommand.Create(OpenCreatingApplicationPageImpl);
        OpenEmployeesPage = ReactiveCommand.Create(OpenEmployeesPageImpl);
        OpenApplicationsPage = ReactiveCommand.Create(OpenApplicationsPageImpl);
        OpenDealListPage = ReactiveCommand.Create(OpenDealListPageImpl);
        LogoutCommand = ReactiveCommand.Create(LogoutImpl);
    }
    
    private void OnNavigationRequested(PageViewModelBase page)
    {
        if (page != null)
        {
            SelectedPageItem = page;
        }
    }

    private void LogoutImpl()
    {
        CurrentAuth = null;
        MessageBus.Current.SendMessage<Employee?>(null, "CurrentAuth");
        
        _windowService.Logout();
    }

    private void OpenHomePageImpl() => SelectedPageItem = PaneItems[2];
    private void OpenMyProfilePageImpl() => SelectedPageItem = PaneItems[0];
    private void OpenCreatingApplicationPageImpl() => SelectedPageItem = PaneItems[3];
    private void OpenEmployeesPageImpl() => SelectedPageItem = PaneItems[4];
    private void OpenApplicationsPageImpl() => SelectedPageItem = PaneItems[5];
    private void OpenDealListPageImpl() => SelectedPageItem = PaneItems[6];

    private void GetDataFromMessageBus()
    {
        MessageBus.Current
            .Listen<Employee>("CurrentAuth")
            .Subscribe(x => { CurrentAuth = x; });
        
        MessageBus.Current
            .Listen<PageViewModelBase>("NavigateToPage")
            .ObserveOn(RxApp.MainThreadScheduler) // Важно для UI
            .Subscribe(page => 
            {
                if (page != null)
                {
                    SelectedPageItem = page;
                }
            });
    }
    
    public void Dispose()
    {
        _navigationService.OnNavigationRequested -= OnNavigationRequested;
    }
}