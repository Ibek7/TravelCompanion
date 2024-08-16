using Microsoft.Maui.Controls;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class NewTripPage : BasePage
    {
        private readonly TripViewModel _viewModel;

        public NewTripPage(TripViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
            _viewModel = viewModel;
        }

        private async void OnNewTripClicked(object sender, EventArgs e)
        {
            // Ensure that the SelectedTrip is properly set
            _viewModel.SelectedTrip = new TripDto
            {
                LodgingName = lodgingNameEntry.Text,
                LodgingAddress = lodgingAddressEntry.Text,
                LodgingCity = lodgingCityEntry.Text,
                LodgingState = lodgingStateEntry.Text,
                LodgingCountry = lodgingCountryEntry.Text,
                LodgingPostalCode = lodgingPostalCodeEntry.Text,
                ArrivalDate = arrivalDatePicker.Date,
                DepartureDate = departureDatePicker.Date,
                TripNotes = tripNotesEditor.Text
            };

            // Call the AddTrip method from ViewModel
            await _viewModel.AddTrip();

            await DisplayAlert("Success", "Trip Created Successfully", "OK");
            await Navigation.PopToRootAsync(); // Navigate back to the home page
        }
    }
}
