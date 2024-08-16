using Microsoft.Extensions.DependencyInjection;
using System.Runtime;
using TravelCompanion.Domain.Services;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.SDK.Extensions
{
    public static class ServiceCollectionExtensions
    {
        internal static ApiSettings Settings;

        private static void AddSdkClients(IServiceCollection serviceCollection, Action<ApiSettings> optionsFunc)
        {
            Settings = new ApiSettings();
            optionsFunc(Settings);

            serviceCollection.AddSingleton<TripClient>();
            serviceCollection.AddSingleton<AppUserClient>();
            serviceCollection.AddSingleton<TripChatClient>();
            serviceCollection.AddSingleton<TripEventClient>();

            serviceCollection.AddHttpClient<TripClient>(client =>
            {
                var baseAddress = Settings.BaseAddress;
                if (!baseAddress.AbsoluteUri.EndsWith("/"))
                    baseAddress = new Uri(baseAddress.AbsoluteUri + "/");
                client.BaseAddress = baseAddress;
            });

            serviceCollection.AddHttpClient<AppUserClient>(client =>
            {
                var baseAddress = Settings.BaseAddress;
                if (!baseAddress.AbsoluteUri.EndsWith("/"))
                    baseAddress = new Uri(baseAddress.AbsoluteUri + "/");
                client.BaseAddress = baseAddress;
            });

            serviceCollection.AddHttpClient<TripChatClient>(client =>
            {
                var baseAddress = Settings.BaseAddress;
                if (!baseAddress.AbsoluteUri.EndsWith("/"))
                    baseAddress = new Uri(baseAddress.AbsoluteUri + "/");
                client.BaseAddress = baseAddress;
            });

            serviceCollection.AddHttpClient<TripEventClient>(client =>
            {
                var baseAddress = Settings.BaseAddress;
                if (!baseAddress.AbsoluteUri.EndsWith("/"))
                    baseAddress = new Uri(baseAddress.AbsoluteUri + "/");
                client.BaseAddress = baseAddress;
            });
        }

        public static void AddSdkClients<T>(this IServiceCollection serviceCollection, Action<ApiSettings> optionsFunc, T implementationInstance)
        {
            AddSdkClients(serviceCollection, optionsFunc);

            serviceCollection.AddSingleton(typeof(IApiKeyProvider), implementationInstance);
        }

        public static void AddSdkClients<T>(this IServiceCollection serviceCollection, Action<ApiSettings> optionsFunc) where T : IApiKeyProvider
        {
            AddSdkClients(serviceCollection, optionsFunc);

            serviceCollection.AddSingleton(typeof(IApiKeyProvider), typeof(T));
        }
    }
}
