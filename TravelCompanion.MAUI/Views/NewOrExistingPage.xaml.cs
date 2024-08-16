using Microsoft.Maui.Controls;
using TravelCompanion.MAUI.ViewModels;
using TravelCompanion.MAUI.Models; 

namespace TravelCompanion.MAUI.Views
{
    public partial class NewOrExistingPage : BasePage
    {
        public NewOrExistingPage()
        {
            InitializeComponent();
        }

        private async void OnNewTripClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(NewTripPage));
        }

        private async void OnExistingTripClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ExistingTripsPage));
        }
    }
}
