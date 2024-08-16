using TravelCompanion.MAUI.Views;

namespace TravelCompanion.MAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AuthenticationPage), typeof(AuthenticationPage));
        Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        Routing.RegisterRoute(nameof(TripPlanningPage), typeof(TripPlanningPage));
        Routing.RegisterRoute(nameof(SchedulePage), typeof(SchedulePage)); 
        Routing.RegisterRoute(nameof(DailySchedulePage), typeof(DailySchedulePage));
        Routing.RegisterRoute(nameof(RealTimeAssistancePage), typeof(RealTimeAssistancePage));
        Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        Routing.RegisterRoute(nameof(MemoryCreationPage), typeof(MemoryCreationPage));
        Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
        Routing.RegisterRoute(nameof(CommunityPage), typeof(CommunityPage));
        Routing.RegisterRoute(nameof(NewOrExistingPage), typeof(NewOrExistingPage));
        Routing.RegisterRoute(nameof(NewTripPage), typeof(NewTripPage));
        Routing.RegisterRoute(nameof(ExistingTripsPage), typeof(ExistingTripsPage));
        Routing.RegisterRoute(nameof(TripDetailPage), typeof(TripDetailPage));
    }
}
