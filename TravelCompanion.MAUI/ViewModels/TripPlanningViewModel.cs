using System.Windows.Input;

namespace TravelCompanion.MAUI.ViewModels
{
    public class TripPlanningViewModel : BaseViewModel
    {
        public string Destination { get; set; }
        public string Duration { get; set; }
        public string Budget { get; set; }
        public string Interests { get; set; }

        public string NumberOfPeople { get; set;}

        public ICommand SubmitTripCommand => new Command(() =>
        {
            // Save trip information logic
        });
    }
}
