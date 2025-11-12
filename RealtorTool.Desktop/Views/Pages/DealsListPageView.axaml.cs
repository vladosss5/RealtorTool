using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class DealsListPageView : UserControl
{
    public DealsListPageView()
    {
        InitializeComponent();
    }

    public DealsListPageView(
        DealsListPageViewModel viewModel) 
        : this()
    {
        DataContext = viewModel;
    }
}