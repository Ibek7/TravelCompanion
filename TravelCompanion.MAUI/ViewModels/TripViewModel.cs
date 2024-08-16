using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using TravelCompanion.Domain.DTOs;
using Microsoft.Maui.Controls;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.MAUI.ViewModels
{
    public class TripViewModel : INotifyPropertyChanged
    {
        private readonly TripClient _tripClient;

        private TripDto _selectedTrip;

        public ObservableCollection<TripDto> Trips { get; set; } = new ObservableCollection<TripDto>();

        public TripDto SelectedTrip
        {
            get => _selectedTrip;
            set
            {
                if (_selectedTrip != value)
                {
                    _selectedTrip = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand LoadTripsCommand { get; }
        public ICommand AddTripCommand { get; }
        public ICommand UpdateTripCommand { get; }
        public ICommand DeleteTripCommand { get; }

        public TripViewModel(TripClient tripClient)
        {
            _tripClient = tripClient;

            LoadTripsCommand = new Command(async () => await LoadTrips());
            AddTripCommand = new Command(async () => await AddTrip());
            UpdateTripCommand = new Command(async () => await UpdateTrip());
            DeleteTripCommand = new Command(async () => await DeleteTrip());
        }

        public async Task LoadTrips()
        {
            var trips = await _tripClient.GetAllTripsForCurrentUser();
            Trips.Clear();
            foreach (var trip in trips)
            {
                Trips.Add(trip);
            }
        }

        public async Task AddTrip()
        {
            if (SelectedTrip != null)
            {
                var newTrip = await _tripClient.CreateTripAsync(SelectedTrip);
                Trips.Add(newTrip);
            }
        }

        public async Task UpdateTrip()
        {
            if (SelectedTrip != null)
            {
                var updatedTrip = await _tripClient.UpdateTripAsync(SelectedTrip.TripId, SelectedTrip);
                var existingTrip = Trips.FirstOrDefault(t => t.TripId == SelectedTrip.TripId);
                if (existingTrip != null)
                {
                    var index = Trips.IndexOf(existingTrip);
                    Trips[index] = updatedTrip;
                }
            }
        }

        public async Task DeleteTrip()
        {
            if (SelectedTrip != null)
            {
                var success = await _tripClient.DeleteTripAsync(SelectedTrip.TripId);
                if (success)
                {
                    Trips.Remove(SelectedTrip);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
