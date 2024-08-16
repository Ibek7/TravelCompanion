using Microsoft.Maui.Controls;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.MAUI.Views
{
    [QueryProperty(nameof(TripId), "TripId")]
    public partial class TripDetailPage : BasePage
    {
        private readonly TripClient _tripClient;

        public TripDetailPage(TripClient tripClient)
        {
            InitializeComponent();
            _tripClient = tripClient;
        }

        private int _tripId;

        public int TripId
        {
            get => _tripId;
            set
            {
                _tripId = value;
                LoadTripAsync(_tripId);
            }
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (_tripId > 0)
            {
                await LoadTripAsync(_tripId);
            }
        }

        private async Task LoadTripAsync(int tripId)
        {
            var selectedTrip = await _tripClient.GetTripByIdAsync(tripId);
            BindingContext = selectedTrip;
        }

        private async void OnChatButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync(nameof(ChatPage) + "?TripId=" + TripId.ToString());
        }

        private async void OnHomeButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to HomePage
            await Shell.Current.GoToAsync("//HomePage");
        }

        private async void OnAnalyzeImageButtonClicked(object sender, EventArgs e)
        {
            // Navigate back to HomePage
            await Shell.Current.GoToAsync("//ImageAnalyzerPage");
        }
    }
}
