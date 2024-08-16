using TravelCompanion.MAUI.ViewModels;


namespace TravelCompanion.MAUI.Views
{
    public partial class RealTimeAssistancePage : BasePage
    {
        public RealTimeAssistancePage()
        {
            InitializeComponent();
            BindingContext = new RealTimeAssistanceViewModel();
        }
    }
}
