using Microsoft.Extensions.DependencyInjection;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.Tests
{
    [TestClass]
    public class TripEventServiceTests : BaseTests
    {
        [TestMethod]
        public void GetCreateTripEventJson()
        {
            var tripService = ServiceProvider.GetRequiredService<TripService>();
            var trip = tripService.GetTripDtoByIdAsync(1).Result; // Adjust with a valid TripId for testing

            var tripEventService = ServiceProvider.GetRequiredService<TripEventService>();
            var tripEventDto = new TripEventDto
            {
                TripId = trip.TripId,
                EventName = "Test Event",
                VenueName = "Test Venue",
                VenueAddress = "123 Main St",
                VenueCity = "Orlando",
                VenueState = "FL",
                VenueCountry = "USA",
                VenuePostalCode = "32830",
                StartDateTime = new DateTime(2024, 12, 1, 18, 0, 0),
                EndDateTime = new DateTime(2024, 12, 1, 20, 0, 0),
                EventNotes = "This is a test event."
            };

            var json = tripEventDto.ConvertToJson();
            Console.WriteLine(json);
        }

        [TestMethod]
        public void TestCreateTripEvent()
        {
            var tripService = ServiceProvider.GetRequiredService<TripService>();
            var trip = tripService.GetTripDtoByIdAsync(1).Result; // Adjust with a valid TripId for testing

            var tripEventService = ServiceProvider.GetRequiredService<TripEventService>();
            var tripEventDto = new TripEventDto
            {
                TripId = trip.TripId,
                EventName = "Test Event",
                VenueName = "Test Venue",
                VenueAddress = "123 Main St",
                VenueCity = "Orlando",
                VenueState = "FL",
                VenueCountry = "USA",
                VenuePostalCode = "32830",
                StartDateTime = new DateTime(2024, 12, 1, 18, 0, 0),
                EndDateTime = new DateTime(2024, 12, 1, 20, 0, 0),
                EventNotes = "This is a test event."
            };
            var addedTripEvent = tripEventService.CreateTripEventAsync(tripEventDto).Result;
            Assert.IsNotNull(addedTripEvent);
            Assert.IsTrue(addedTripEvent.TripEventId > 0);
            Console.WriteLine(addedTripEvent.ConvertToJson());
        }

        [TestMethod]
        public void TestUpdateTripEvent()
        {
            var tripEventService = ServiceProvider.GetRequiredService<TripEventService>();
            var tripEvent = tripEventService.GetTripEventByIdAsync(1).Result; // Adjust with a valid TripEventId for testing
            Assert.IsNotNull(tripEvent);

            tripEvent.EventName = "Updated Event Name";

            var updatedTripEvent = tripEventService.UpdateTripEventAsync(tripEvent).Result;
            Assert.IsNotNull(updatedTripEvent);
            Assert.AreEqual("Updated Event Name", updatedTripEvent.EventName);
            Console.WriteLine(updatedTripEvent.ConvertToJson());
        }

        [TestMethod]
        public void TestDeleteTripEvent()
        {
            var tripEventService = ServiceProvider.GetRequiredService<TripEventService>();
            var result = tripEventService.DeleteTripEventAsync(1).Result; // Adjust with a valid TripEventId for testing
            Assert.IsTrue(result);
            Console.WriteLine("TripEvent deleted successfully.");
        }
    }
}
