using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class CreatingApplicationPageView : UserControl
{
    public CreatingApplicationPageView()
    {
        InitializeComponent();
    }

    public CreatingApplicationPageView(
        CreatingApplicationPageViewModel viewModel)
        : this()
    {
        DataContext = viewModel;
    }
}