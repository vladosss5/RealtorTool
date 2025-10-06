using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class HomePageView : UserControl
{
    public HomePageView()
    {
        InitializeComponent();
    }
    
    public HomePageView(HomePageViewModel viewModel) 
        : this()
    {
        DataContext = viewModel;
    }
}