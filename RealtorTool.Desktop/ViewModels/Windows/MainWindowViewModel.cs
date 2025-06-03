using System;
using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbModels;
using RealtorTool.Core.DbModels.PersonModels;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<PageViewModelBase> PaneItems { get; set; }
    
    [Reactive] public PageViewModelBase SelectedPageItem { get; set; }
    
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    private AuthorizationData CurrentAuth { get; set; }

    public MainWindowViewModel(
        MyProfilePageViewModel myProfilePageViewModel,
        PersonProfilePageViewModel personProfilePageViewModel)
    {
        PaneItems = (
        [
            myProfilePageViewModel,
            personProfilePageViewModel
        ]);

        SelectedPageItem = PaneItems[0];
            
        MessageBus.Current
            .Listen<AuthorizationData>("CurrentAuth")
            .Subscribe(x => 
            {
                CurrentAuth = x;
            });
    }
}