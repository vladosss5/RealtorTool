using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RealtorTool.Desktop.ViewModels;
using RealtorTool.Services.Interfaces;

namespace RealtorTool.Desktop.Services.Implementations;

public class NavigationService : INavigationService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly Stack<ViewModelBase> _navigationStack = new();
    
    // Добавьте событие для уведомления о смене страницы
    public event Action<ViewModelBase>? CurrentPageChanged;

    public NavigationService(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        await Navigate(viewModel);
    }

    public async Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
    {
        var viewModel = _serviceProvider.GetRequiredService<TViewModel>();
        
        if (viewModel is IParameterReceiver parameterReceiver)
        {
            parameterReceiver.ReceiveParameter(parameter);
        }
        await Navigate(viewModel);
    }

    private async Task Navigate(ViewModelBase viewModel)
    {
        _navigationStack.Push(viewModel);
        
        // УВЕДОМИТЕ о смене страницы
        CurrentPageChanged?.Invoke(viewModel);
        
        await Task.CompletedTask;
    }

    public async Task GoBackAsync()
    {
        if (_navigationStack.Count > 1)
        {
            _navigationStack.Pop();
            var previousViewModel = _navigationStack.Peek();
            CurrentPageChanged?.Invoke(previousViewModel);
        }
        await Task.CompletedTask;
    }
}