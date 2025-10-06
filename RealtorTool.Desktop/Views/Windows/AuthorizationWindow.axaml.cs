using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.Views.Windows;

public partial class AuthorizationWindow : Window
{
    public AuthorizationWindow(
        AuthorizationWindowViewModel viewModel,
        IWindowService windowService)
    {
        InitializeComponent();
        DataContext = viewModel;
        windowService.SetCurrentWindow(this);
    }
}