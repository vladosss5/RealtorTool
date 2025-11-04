using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using RealtorTool.Core.DbEntities.Views;
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

    public bool CanCreateDeals => (DataContext as ListingDetailViewModel)?.CanCreateDeals ?? false;

    public async void CreateDeal(PotentialMatch selectedMatch)
    {
        await (DataContext as ListingDetailViewModel).CreateDealAsync(selectedMatch);
    }
}