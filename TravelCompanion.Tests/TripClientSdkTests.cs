using Microsoft.Extensions.DependencyInjection;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.Domain.Services;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.Tests
{
    /// <summary>
    /// Goes through the SDK to test the TripClient. The SDK will communicate through the
    /// web API with connection string defined in Configuration["TravelCompanionBaseAddress"].
    /// </summary>
    [TestClass]
    public class TripClientSdkTests : BaseTests
    {
        [TestMethod]
        public void TestGetTrips()
        {
            var tripClient = ServiceProvider.GetRequiredService<TripClient>();
            var trips = tripClient.GetAllTripsForCurrentUser().Result;
            Console.WriteLine(trips.ConvertToJson());
        }

        [TestMethod]
        public void TestCreateTrip()
        {
            var appUserService = ServiceProvider.GetRequiredService<AppUserService>();
            var appUser = appUserService.GetByEmailAsync("YOUR@gmail.com").Result; // add your email address

            var tripClient = ServiceProvider.GetRequiredService<TripClient>();
            var tripDto = new TripDto
            {
                AppUserId = appUser.AppUserId,
                ArrivalDate = new DateTime(2024, 12, 1),
                DepartureDate = new DateTime(2024, 12, 15),
                LodgingCity = "Orlando",
                LodgingCountry = "USA",
                LodgingName = "Disney's Grand Floridian Resort & Spa",
                LodgingPostalCode = "32830",
                LodgingState = "FL",
                TripNotes = "Would like to visit theme parks and seafood restaurants"
            };
            var addedTrip = tripClient.CreateTripAsync(tripDto).Result;
            Console.WriteLine(addedTrip.ConvertToJson());
        }
    }
}
