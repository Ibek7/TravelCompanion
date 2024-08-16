using System.ComponentModel;
using System.Runtime.CompilerServices;
using TravelCompanion.MAUI.Views;

namespace TravelCompanion.MAUI.ViewModels
{
    public class BaseViewModel : ObservableObject
    {
        bool isBusy;

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value, onChanged: () => OnPropertyChanged(nameof(IsNotBusy)));
        }

        public bool IsNotBusy => !IsBusy;

        public virtual void OnAppearing()
        {
        }

        public virtual void OnDisappearing()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal event Func<string, Task> DoDisplayAlert;

        internal event Func<BaseViewModel, bool, Task> DoNavigate;

        public Task DisplayAlertAsync(string message)
        {
            return DoDisplayAlert?.Invoke(message) ?? Task.CompletedTask;
        }

        public Task NavigateAsync(BaseViewModel vm, bool showModal = false)
        {
            return DoNavigate?.Invoke(vm, showModal) ?? Task.CompletedTask;
        }
    }
}
