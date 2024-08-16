using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class SettingsPage : BasePage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void OnEditProfileClicked(object sender, EventArgs e)
        {
            // Navigate to the Edit Profile page
           
        }

        private void OnPrivacySettingsClicked(object sender, EventArgs e)
        {
            // Navigate to the Privacy Settings page
           
        }

        private void OnThemeToggled(object sender, ToggledEventArgs e)
        {
            // Toggle between light and dark theme
            Application.Current.UserAppTheme = e.Value ? AppTheme.Dark : AppTheme.Light;
        }
    }
}
