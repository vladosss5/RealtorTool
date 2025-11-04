using Avalonia.Controls;
using RealtorTool.Desktop.Services.Interfaces;
using RealtorTool.Desktop.ViewModels.Windows;

namespace RealtorTool.Desktop.Views.Windows;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(
        IWindowService windowService, 
        MainWindowViewModel viewModel) : this()
    {
        DataContext = viewModel;
        windowService.SetCurrentWindow(this);
    }
}