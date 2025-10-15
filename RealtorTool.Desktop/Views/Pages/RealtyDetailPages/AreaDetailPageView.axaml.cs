using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;

namespace RealtorTool.Desktop.Views.Pages.RealtyDetailPages;

public partial class AreaDetailPageView : UserControl
{
    public AreaDetailPageView()
    {
        InitializeComponent();
    }
    
    public AreaDetailPageView(
        AreaDetailPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}