using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;
using RealtorTool.Core.DbEntities.RealtyModels;

namespace RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;

public class PrivateHouseDetailPageViewModel : PageViewModelBase
{
    [Reactive] public PrivateHouse House { get; set; }
    
    public PrivateHouseDetailPageViewModel()
    {
        Title = "Частный дом";

        MessageBus.Current
            .Listen<PrivateHouse>()
            .Take(1)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(h =>
            {
                House = h;
            });
    }
}