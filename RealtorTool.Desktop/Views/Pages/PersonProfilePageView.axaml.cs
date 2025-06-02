using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class PersonProfilePageView : UserControl
{
    public PersonProfilePageView(
        PersonProfilePageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}