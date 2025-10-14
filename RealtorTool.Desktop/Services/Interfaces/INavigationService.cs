using System.Threading.Tasks;
using RealtorTool.Desktop.ViewModels;

namespace RealtorTool.Services.Interfaces;

public interface INavigationService
{
    public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
    public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
    public Task GoBackAsync();
}