using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TravelCompanion.Domain.Mapping;
using TravelCompanion.Domain.Models;
using TravelCompanion.Domain.Services;
using TravelCompanion.SDK;
using TravelCompanion.SDK.Extensions;

namespace TravelCompanion.Tests
{
    public class BaseTests : IApiKeyProvider
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected IConfiguration Configuration { get; private set; }

        public string ApiBaseAddress { get; set; } = "https://yourapi.azurewebsites.net/"; // Your TravelCompanion API base address
        public string ApiKey { get; set; } = ""; // Your AppUserGuid

        public BaseTests()
        {
            Configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(Configuration);

            services.AddDbContext<TravelCompanionContext>(options =>
            {
                options.UseSqlServer(Configuration["TravelCompanionContext"]);
            }, ServiceLifetime.Transient);
            
            services.AddSdkClients(options =>
            {
                options.BaseAddress = new Uri(ApiBaseAddress);
            }, this);

            services.AddScoped<AppUserService>();
            services.AddScoped<TripService>();
            services.AddScoped<TripChatService>();
            services.AddScoped<TripEventService>();

            services.AddAutoMapper(typeof(MappingProfile));

            ServiceProvider = services.BuildServiceProvider();
        }

        public string GetApiKey()
        {
            return ApiKey;
        }
    }
}