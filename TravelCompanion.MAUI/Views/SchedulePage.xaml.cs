using Microsoft.Maui.Controls;
using TravelCompanion.MAUI.ViewModels;

namespace TravelCompanion.MAUI.Views
{
    public partial class SchedulePage : ContentPage
    {
        public SchedulePage()
        {
            InitializeComponent();
            BindingContext = MauiProgram.ServiceProvider.GetRequiredService<ScheduleLayoutViewModel>();
        }
    }
}
