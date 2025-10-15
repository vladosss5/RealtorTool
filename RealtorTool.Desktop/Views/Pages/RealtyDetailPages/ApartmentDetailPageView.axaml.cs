using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;

namespace RealtorTool.Desktop.Views.Pages.RealtyDetailPages;

public partial class ApartmentDetailPageView : UserControl
{
    public ApartmentDetailPageView()
    {
        InitializeComponent();
    }
    
    public ApartmentDetailPageView(
        ApartmentDetailPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}