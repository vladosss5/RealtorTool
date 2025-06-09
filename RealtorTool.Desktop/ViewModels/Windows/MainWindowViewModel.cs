using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbModels.PersonModels;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.ViewModels.Windows;

/// <summary>
/// VM Главного окна.
/// </summary>
public class MainWindowViewModel : ViewModelBase
{
    private AuthorizationData CurrentAuth { get; set; }
    
    public ObservableCollection<PageViewModelBase> PaneItems { get; set; }
    
    [Reactive] public PageViewModelBase SelectedPageItem { get; set; }
    
    public ICommand OpenMyProfilePage { get; private set; }
    public ICommand OpenHomePage { get; private set; }
    public bool CanOpenMyProfile { get; } = false;

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MainWindowViewModel(
        MyProfilePageViewModel myProfilePageViewModel,
        PersonProfilePageViewModel personProfilePageViewModel,
        HomePageViewModel homePageViewModel)
    {
        PaneItems = 
        [
            myProfilePageViewModel, 
            personProfilePageViewModel, 
            homePageViewModel
        ];
        SelectedPageItem = PaneItems[2];
        
        InitialButtons();
        GetDataFromMessageBus();
    }

    private void InitialButtons()
    {
        OpenMyProfilePage = ReactiveCommand.Create(OpenMyProfilePageImpl);
        OpenHomePage = ReactiveCommand.Create(OpenHomePageImpl);
    }
    private void OpenHomePageImpl() => SelectedPageItem = PaneItems[2];
    private void OpenMyProfilePageImpl() => SelectedPageItem = PaneItems[0];

    private void GetDataFromMessageBus()
    {
        MessageBus.Current
            .Listen<AuthorizationData>("CurrentAuth")
            .Subscribe(x => 
            {
                CurrentAuth = x;
            });
    }
}