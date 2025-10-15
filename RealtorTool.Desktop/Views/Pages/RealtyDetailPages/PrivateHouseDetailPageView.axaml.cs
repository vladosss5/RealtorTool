using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;

namespace RealtorTool.Desktop.Views.Pages.RealtyDetailPages;

public partial class PrivateHouseDetailPageView : UserControl
{
    public PrivateHouseDetailPageView()
    {
        InitializeComponent();
    }
    
    public PrivateHouseDetailPageView(
        PrivateHouseDetailPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}