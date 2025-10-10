using System;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.Enums;
using RealtorTool.Desktop.ViewModels.Pages.ApplicationPages;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class CreatingApplicationPageViewModel : PageViewModelBase
{
    [Reactive] public bool RentIsVisible { get; set; } = true;
    [Reactive] public bool RentingOutIsVisible { get; set; }
    [Reactive] public bool PurchaseIsVisible { get; set; }
    [Reactive] public bool SaleIsVisible { get; set; }
    
    private readonly Lazy<BuyApplicationPageViewModel> _buyApplicationPageViewModel;
    private readonly Lazy<SellApplicationPageViewModel> _sellApplicationPageViewModel;
    private readonly Lazy<LeaseApplicationPageViewModel> _leaseApplicationPageViewModel;
    private readonly Lazy<RentalApplicationPageViewModel> _rentalApplicationPageViewModel;

    public BuyApplicationPageViewModel BuyApplicationPageViewModel => _buyApplicationPageViewModel.Value;
    public SellApplicationPageViewModel SellApplicationPageViewModel => _sellApplicationPageViewModel.Value;
    public LeaseApplicationPageViewModel LeaseApplicationPageViewModel => _leaseApplicationPageViewModel.Value;
    public RentalApplicationPageViewModel RentalApplicationPageViewModel => _rentalApplicationPageViewModel.Value;
    
    public ReactiveCommand<ApplicationType, Unit> ChangeOperationType { private set; get; }

    public CreatingApplicationPageViewModel(
        Func<BuyApplicationPageViewModel> buyApplicationPageViewModelFactory,
        Func<SellApplicationPageViewModel> sellApplicationPageViewModelFactory,
        Func<LeaseApplicationPageViewModel> leaseApplicationPageViewModelFactory,
        Func<RentalApplicationPageViewModel> rentalApplicationPageViewModelFactory)
    {
        InitialButtons();
        
        _buyApplicationPageViewModel = new Lazy<BuyApplicationPageViewModel>(buyApplicationPageViewModelFactory);
        _sellApplicationPageViewModel = new Lazy<SellApplicationPageViewModel>(sellApplicationPageViewModelFactory);
        _leaseApplicationPageViewModel = new Lazy<LeaseApplicationPageViewModel>(leaseApplicationPageViewModelFactory);
        _rentalApplicationPageViewModel = new Lazy<RentalApplicationPageViewModel>(rentalApplicationPageViewModelFactory);
    }

    private void InitialButtons()
    {
        ChangeOperationType = ReactiveCommand.Create<ApplicationType>(ChangeOperationTypeImpl);
    }

    private void ChangeOperationTypeImpl(ApplicationType type)
    {
        RentIsVisible = type == ApplicationType.Rent;
        RentingOutIsVisible = type == ApplicationType.RentOut;
        PurchaseIsVisible = type == ApplicationType.Purchase;
        SaleIsVisible = type == ApplicationType.Sale;
    }
}