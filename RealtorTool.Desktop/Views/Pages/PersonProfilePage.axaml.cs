using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class PersonProfilePage : UserControl
{
    public PersonProfilePage(
        PersonProfilePageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}