using System;
using ReactiveUI;
using RealtorTool.Core.DbModels;

namespace RealtorTool.Desktop.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to Avalonia!";
    
    private AuthorizationData CurrentAuth { get; set; }

    public MainWindowViewModel()
    {
        MessageBus.Current
            .Listen<AuthorizationData>("CurrentAuth")
            .Subscribe(x => 
            {
                CurrentAuth = x;
            });
    }
}