using Microsoft.Extensions.DependencyInjection;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.Tests
{
    [TestClass]
    public class TripServiceTests : BaseTests
    {
        [TestMethod]
        public void GetCreateTripJson()
        {
            var appUserService = ServiceProvider.GetRequiredService<AppUserService>();
          
            var appUser = appUserService.GetByEmailAsync("USER@gmail.com").Result; // add your email address here

            var tripService = ServiceProvider.GetRequiredService<TripService>();
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

            var json = tripDto.ConvertToJson();
            Console.WriteLine(json);
        }

        [TestMethod]
        public void TestCreateTrip()
        {
            var appUserService = ServiceProvider.GetRequiredService<AppUserService>();
            var appUser = appUserService.GetByEmailAsync("YOUR@gmail.com").Result; // add your email here

            var tripService = ServiceProvider.GetRequiredService<TripService>();
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
            var addedTrip = tripService.CreateTripAsync(tripDto).Result;
            Assert.IsNotNull(addedTrip); 
            Assert.IsTrue(addedTrip.TripId > 0);
            Console.WriteLine(tripDto.ConvertToJson());
        }
    }
}
