using Avalonia.Controls;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class EmployeesPageView : UserControl
{
    public EmployeesPageView()
    {
        InitializeComponent();
    }

    public EmployeesPageView(HomePageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}