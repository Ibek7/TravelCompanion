using TravelCompanion.MAUI.ViewModels;



namespace TravelCompanion.MAUI.Views
{
    public partial class AuthenticationPage : BasePage
    {
        public AuthenticationPage()
        {
            InitializeComponent();
            BindingContext = new AuthenticationViewModel();
        }
    }
}
