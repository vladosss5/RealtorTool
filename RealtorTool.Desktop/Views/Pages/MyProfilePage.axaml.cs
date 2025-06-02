using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class MyProfilePage : UserControl
{
    public MyProfilePage(
        MyProfilePageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}