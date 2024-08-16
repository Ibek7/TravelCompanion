using CommunityToolkit.Mvvm.ComponentModel;
using Syncfusion.Maui.Scheduler;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.MAUI.ViewModels
{
    public partial class ScheduleViewModel : ObservableObject
    {
        private readonly TripClient _tripClient;
 
        [ObservableProperty]
        private ObservableCollection<SchedulerAppointment> _appointments;
 
        public ScheduleViewModel(TripClient tripClient)
        {
            _tripClient = tripClient;
            Appointments = new ObservableCollection<SchedulerAppointment>();
        }
 
        public async Task LoadEventsAsync()
        {
            //var trips = await _tripClient.GetAllTripsForCurrentUser();
            //foreach (var trip in trips)
            {
              //  Appointments.Add(new SchedulerAppointment
                {
                //    Subject = trip.LodgingName,
              //      StartTime = trip.ArrivalDate ?? DateTime.Now,
              //      EndTime = trip.DepartureDate ?? DateTime.Now.AddHours(1),
             //       Location = trip.LodgingAddress
            //    });
            }
        }
    }
    }
}