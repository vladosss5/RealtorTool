using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class ApplicationsPageView : UserControl
{
    public ApplicationsPageView()
    {
        InitializeComponent();
    }
    
    public ApplicationsPageView(
        ApplicationsPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}