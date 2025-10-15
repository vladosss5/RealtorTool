using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using RealtorTool.Core.DbEntities;

namespace RealtorTool.Desktop.ViewModels.Pages.RealtyDetailPages;

public class ApartmentDetailPageViewModel : PageViewModelBase
{
    [Reactive] public Apartment Apartment { get; set; }

    public ApartmentDetailPageViewModel()
    {
        Title = "Квартира";

        MessageBus.Current
            .Listen<Apartment>()
            .Take(1)
            .ObserveOn(RxApp.MainThreadScheduler)
            .Subscribe(a =>
            {
                Apartment = a;
            });
    }
}