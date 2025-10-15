using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;

namespace RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;

public class AreaDetailPageViewModel : PageViewModelBase
{
    [Reactive] public Area Area { get; set; }
    
    public AreaDetailPageViewModel()
    {
        Title = "Район";

        MessageBus.Current
            .Listen<Area>()
            .Take(1)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(a =>
            {
                Area = a;
            });
    }
}