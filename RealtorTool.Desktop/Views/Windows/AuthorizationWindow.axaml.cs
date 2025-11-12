using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.ViewModels;

namespace RealtorTool.Desktop.Views.Windows;

public partial class AuthorizationWindow : Window
{
    public AuthorizationWindow()
    {
        InitializeComponent();
    }

    public AuthorizationWindow(AuthorizationWindowViewModel viewModel,
        IWindowService windowService) : this()
    {
        DataContext = viewModel;
        windowService.SetCurrentWindow(this);
    }
}