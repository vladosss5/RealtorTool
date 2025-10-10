using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

namespace RealtorTool.Desktop.Views.Pages.ApplicationPages;

public partial class LeaseApplicationPageView : UserControl
{
    public LeaseApplicationPageView()
    {
        InitializeComponent();
    }
    
    public LeaseApplicationPageView(
        LeaseApplicationPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}