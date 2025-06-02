using Avalonia.Controls;
using RealtorTool.Desktop.ViewModels;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.Views.Windows;

public partial class MainWindow : Window
{
    public MainWindow(
        IWindowService windowService, 
        MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        windowService.SetCurrentWindow(this);
    }
}