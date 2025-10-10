using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

namespace RealtorTool.Desktop.Views.Pages.ApplicationPages;

public partial class RentalApplicationPageView : UserControl
{
    public RentalApplicationPageView()
    {
        InitializeComponent();
    }
    
    public RentalApplicationPageView(
        RentalApplicationPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}