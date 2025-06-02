using Avalonia.Controls;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.Views.Windows;

public partial class MainWindow : Window
{
    public MainWindow(IWindowService windowService)
    {
        InitializeComponent();
        windowService.SetCurrentWindow(this);
    }
}