using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using TravelCompanion.Domain.Models;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.MAUI.ViewModels
{
    public class ScheduleLayoutViewModel
    {
        public ObservableCollection<ScheduleInfo> Data { get; set; }

        private readonly TripClient _tripClient;
        private readonly TripEventClient _tripEventClient;

        public ScheduleLayoutViewModel(TripClient tripClient, TripEventClient tripEventClient)
        {
            Data = new ObservableCollection<ScheduleInfo>();
            _tripClient = tripClient;
            _tripEventClient = tripEventClient;
            GenerateSource();
        }

        private async Task GenerateSource()
        {
            var trips = await _tripClient.GetAllTripsForCurrentUser();
            foreach (var trip in trips)
            {
                // build trip description from trip event's lodging name, address, city, and state
                // any of those properties could be null or empty; skip them if so
                var tripDescription = new StringBuilder();
                if (!string.IsNullOrEmpty(trip.LodgingName))
                {
                    tripDescription.Append(trip.LodgingName);
                }
                if (!string.IsNullOrEmpty(trip.LodgingAddress))
                {
                    if (tripDescription.Length > 0)
                    {
                        tripDescription.Append(", ");
                    }
                    tripDescription.Append(trip.LodgingAddress);
                }
                if (!string.IsNullOrEmpty(trip.LodgingCity))
                {
                    if (tripDescription.Length > 0)
                    {
                        tripDescription.Append(", ");
                    }
                    tripDescription.Append(trip.LodgingCity);
                }
                if (!string.IsNullOrEmpty(trip.LodgingState))
                {
                    if (tripDescription.Length > 0)
                    {
                        tripDescription.Append(", ");
                    }
                    tripDescription.Append(trip.LodgingState);
                }

                Data.Add(new ScheduleInfo
                {
                    From = trip.ArrivalDate,
                    To = trip.DepartureDate,
                    EventName = trip.LodgingCity,
                    EventDescription = tripDescription.ToString()
                });

                var tripEvents = await _tripEventClient.GetAllTripEventsForTripAsync(trip.TripId);
                foreach (var tripEvent in tripEvents)
                {
                    // build event description from trip event's venue name, address, city, and state
                    // any of those properties could be null or empty; skip them if so
                    var eventDescription = new StringBuilder();
                    if (!string.IsNullOrEmpty(tripEvent.VenueName))
                    {
                        eventDescription.Append(tripEvent.VenueName);
                    }
                    if (!string.IsNullOrEmpty(tripEvent.VenueAddress))
                    {
                        if (eventDescription.Length > 0)
                        {
                            eventDescription.Append(", ");
                        }
                        eventDescription.Append(tripEvent.VenueAddress);
                    }
                    if (!string.IsNullOrEmpty(tripEvent.VenueCity))
                    {
                        if (eventDescription.Length > 0)
                        {
                            eventDescription.Append(", ");
                        }
                        eventDescription.Append(tripEvent.VenueCity);
                    }
                    if (!string.IsNullOrEmpty(tripEvent.VenueState))
                    {
                        if (eventDescription.Length > 0)
                        {
                            eventDescription.Append(", ");
                        }
                        eventDescription.Append(tripEvent.VenueState);
                    }

                    Data.Add(new ScheduleInfo
                    {
                        From = tripEvent.StartDateTime,
                        To = tripEvent.EndDateTime,
                        EventName = tripEvent.EventName,
                        EventDescription = eventDescription.ToString()
                    });
                }
            }
        }
    }
}
