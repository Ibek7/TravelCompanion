using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class DailySchedulePage : BasePage
    {
        public DailySchedulePage()
        {
            InitializeComponent();
            BindingContext = new DailyScheduleViewModel();
        }
    }
}
