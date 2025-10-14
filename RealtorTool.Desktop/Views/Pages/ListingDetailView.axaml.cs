using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Desktop.ViewModels.Pages;

namespace RealtorTool.Desktop.Views.Pages;

public partial class ListingDetailView : UserControl
{
    public ListingDetailView()
    {
        InitializeComponent();
    }
    
    public ListingDetailView(ListingDetailViewModel viewModel) 
        : this()
    {
        DataContext = viewModel;
    }
}