using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class HomePage : BasePage
    {
       public HomePage()
        {
            InitializeComponent();
            AnimateImages();
        }

        private async void AnimateImages()
        {
             while (true)
            {
                var tasks = new List<Task>();
                foreach (VisualElement child in imageGrid.Children)
                {
                    tasks.Add(child.TranslateTo(0, -360, 10000, Easing.Linear));
                }

                await Task.WhenAll(tasks);

                tasks.Clear();

                foreach (VisualElement child in imageGrid.Children)
                {
                    tasks.Add(child.FadeTo(0, 2000));
                }

                await Task.WhenAll(tasks);

                foreach (VisualElement child in imageGrid.Children)
                {
                    child.TranslationY = 360;
                    tasks.Add(child.FadeTo(1, 2000));
                }

                await Task.WhenAll(tasks);
            }
        }

        private async void OnTripPlanningClicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Select Trip Option", "Cancel", null, "New", "Existing");
            if (action == "New")
            {
                await Navigation.PushAsync(MauiProgram.ServiceProvider.GetRequiredService<NewTripPage>());
            }
            else if (action == "Existing")
            {
                await Navigation.PushAsync(MauiProgram.ServiceProvider.GetRequiredService<ExistingTripsPage>());
            }
        }

        private void OnHamburgerMenuClicked(object sender, EventArgs e)
        {
            HamburgerMenu.IsVisible = true;
        }

        private void OnCloseMenuClicked(object sender, EventArgs e)
        {
            HamburgerMenu.IsVisible = false;
        }

        private void OnProfileClicked(object sender, EventArgs e)
        {
            // Navigate to the Profile Page
            Navigation.PushAsync(MauiProgram.ServiceProvider.GetRequiredService<ProfilePage>());
        }

        private void OnSettingsClicked(object sender, EventArgs e)
        {
            // Navigate to the Settings page
            Navigation.PushAsync(new SettingsPage());
        }

        private async void OnDailyScheduleClicked(object sender, EventArgs e)
        {
            // Navigate to the Daily Schedule page
            await Shell.Current.GoToAsync("SchedulePage");
        }
    }
}
