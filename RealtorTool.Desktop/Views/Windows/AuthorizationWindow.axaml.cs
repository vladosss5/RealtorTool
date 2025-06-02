using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.Views.Windows;

public partial class AuthorizationWindow : Window
{
    public AuthorizationWindow(IWindowService windowService)
    {
        InitializeComponent();
        windowService.SetCurrentWindow(this);
    }
}