using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class DealDetailPageView : UserControl
{
    public DealDetailPageView()
    {
        InitializeComponent();
    }

    public DealDetailPageView(
        DealDetailPageViewModel viewModel) 
        : this()
    {
        DataContext = viewModel;
    }
}