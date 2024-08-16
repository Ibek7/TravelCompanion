using System.Windows.Input;

namespace TravelCompanion.MAUI.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
         public ICommand NavigateCommand => new Command<string>(async (page) =>
            {
                await Shell.Current.GoToAsync($"/{page}");
            });
    }

 
}
