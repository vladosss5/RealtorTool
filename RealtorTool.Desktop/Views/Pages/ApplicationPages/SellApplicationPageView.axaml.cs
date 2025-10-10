using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

namespace RealtorTool.Desktop.Views.Pages.ApplicationPages;

public partial class SellApplicationPageView : UserControl
{
    public SellApplicationPageView()
    {
        InitializeComponent();
    }
    
    public SellApplicationPageView(
        SellApplicationPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}