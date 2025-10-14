using Avalonia.Controls;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.ViewModels.Windows;

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