using System;
using System.Reactive;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.Enums;

namespace RealtorTool.Desktop.ViewModels.Pages;

public class CreatingApplicationPageViewModel : PageViewModelBase
{
    [Reactive] public bool RentIsVisible { get; set; } = true;
    [Reactive] public bool RentingOutIsVisible { get; set; }
    [Reactive] public bool PurchaseIsVisible { get; set; }
    [Reactive] public bool SaleIsVisible { get; set; }
    
    public ReactiveCommand<ApplicationType, Unit> ChangeOperationType { private set; get; }

    public CreatingApplicationPageViewModel()
    {
        InitialButtons();
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