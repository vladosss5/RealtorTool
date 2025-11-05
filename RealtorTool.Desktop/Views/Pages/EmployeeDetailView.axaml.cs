using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class EmployeeDetailView : UserControl
{
    public EmployeeDetailView()
    {
        InitializeComponent();
    }

    public EmployeeDetailView(EmployeeDetailViewModel viewModel) 
        : this()
    {
        DataContext = viewModel;
    }
}