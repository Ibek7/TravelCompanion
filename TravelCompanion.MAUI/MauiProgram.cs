using Microsoft.Extensions.Logging;
using OpenAI.Chat;
using TravelCompanion.Domain.Models;
using TravelCompanion.SDK;
using TravelCompanion.MAUI.Views;
using TravelCompanion.MAUI.ViewModels;
using TravelCompanion.ViewModels;
using Syncfusion.Licensing;
using Syncfusion.Maui.Core.Hosting;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.SDK.Clients;
using TravelCompanion.SDK.Extensions;

namespace TravelCompanion.MAUI
{
    public static class MauiProgram
    {
        public static string ApiKey { get; set; }

        public static IServiceProvider ServiceProvider;

        public static AppUserDto CurrentUser { get; set; } 

        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    SyncfusionLicenseProvider.RegisterLicense(Constants.SyncfusionLicenseKey);
                });
            builder.ConfigureSyncfusionCore();

            builder.Services.AddTransient<TripViewModel>();
            builder.Services.AddTransient<TripPlanningViewModel>();
            builder.Services.AddTransient<ExistingTripsPage>();
            builder.Services.AddTransient<NewTripPage>();
            builder.Services.AddTransient<ProfileViewModel>();
            builder.Services.AddTransient<ProfilePage>();
            builder.Services.AddTransient<TripDetailPage>();
            builder.Services.AddTransient<ChatPage>();
            builder.Services.AddTransient<ImageAnalyzerPage>();
            builder.Services.AddTransient<SchedulePage>();
            builder.Services.AddTransient<ScheduleLayoutViewModel>();

            builder.Services.AddSingleton<IMediaPicker>(MediaPicker.Default);
            
            builder.Services.AddTransient<ChatClient>(c => new ChatClient(Constants.OpenAiModel, Constants.OpenAiApiKey));
            
            //builder.Services.AddScoped<DocumentIntelligenceService>();
            //builder.Services.AddScoped<SpeechService>();
            //builder.Services.AddScoped<VisionService>();
            //builder.Services.AddScoped<ChatService>();

            builder.Services.AddSdkClients<ApiKeyProvider>(options =>
            {
                options.BaseAddress = new Uri(Constants.ApiBaseAddress);
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            var myApp = builder.Build();

            ServiceProvider = myApp.Services;

            return myApp;
        }
    }
}
