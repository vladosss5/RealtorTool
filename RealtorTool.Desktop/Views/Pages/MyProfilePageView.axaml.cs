using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class MyProfilePageView : UserControl
{
    public MyProfilePageView(
        MyProfilePageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}