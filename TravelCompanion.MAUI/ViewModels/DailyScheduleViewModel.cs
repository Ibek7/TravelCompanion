using System.Collections.ObjectModel;
using System.Windows.Input;
using TravelCompanion.MAUI.Models;
using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.ViewModels
{
    public class DailyScheduleViewModel : BaseViewModel
    {
        public ObservableCollection<ScheduleItem> Schedule { get; set; }

        public DailyScheduleViewModel()
        {
            Schedule = new ObservableCollection<ScheduleItem>
            {
                new ScheduleItem { Task = "Task 1", Time = "9:00 AM" },
                new ScheduleItem { Task = "Task 2", Time = "11:00 AM" }
            };
        }

        public ICommand EditScheduleCommand => new Command(() =>
        {
            // Edit schedule logic
        });
    }
}
