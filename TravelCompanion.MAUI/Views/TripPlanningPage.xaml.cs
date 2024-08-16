using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class TripPlanningPage : BasePage
    {
        public TripPlanningPage()
        {
            InitializeComponent();
            BindingContext = new TripPlanningViewModel();
        }
    }
}
