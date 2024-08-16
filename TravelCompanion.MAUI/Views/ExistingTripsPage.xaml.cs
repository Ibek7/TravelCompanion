using Microsoft.Maui.Controls;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class ExistingTripsPage : BasePage
    {
        private readonly TripViewModel _viewModel;

        public ExistingTripsPage(TripViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadTrips();
        }

        public async void OnTripSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is TripDto selectedTrip)
            {
                _viewModel.SelectedTrip = selectedTrip;

                // Use the TripId in the route to ensure unique navigation
                var tripId = selectedTrip.TripId;  // Assuming TripId is the correct unique identifier
                await Shell.Current.GoToAsync($"{nameof(TripDetailPage)}?TripId={selectedTrip.TripId}");

            }
        }
    }
}
