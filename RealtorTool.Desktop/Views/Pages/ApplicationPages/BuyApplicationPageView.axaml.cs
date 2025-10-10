using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

namespace RealtorTool.Desktop.Views.Pages.ApplicationPages;

public partial class BuyApplicationPageView : UserControl
{
    public BuyApplicationPageView()
    {
        InitializeComponent();
    }
    
    public BuyApplicationPageView(
        BuyApplicationPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}