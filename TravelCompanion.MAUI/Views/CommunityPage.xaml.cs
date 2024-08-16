using TravelCompanion.MAUI.ViewModels;


namespace TravelCompanion.MAUI.Views
{
    public partial class CommunityPage : BasePage
    {
        public CommunityPage()
        {
            InitializeComponent();
            BindingContext = new CommunityViewModel();
        }
    }
}
