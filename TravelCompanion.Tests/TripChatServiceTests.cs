using Microsoft.Extensions.DependencyInjection;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.Domain.Services;

namespace TravelCompanion.Tests
{
    [TestClass]
    public class TripChatServiceTests : BaseTests
    {
        [TestMethod]
        public async Task GetAllTripChatInfoByTripIdAsync()
        {
            var tripChatService = ServiceProvider.GetRequiredService<TripChatService>();
            var tripChats = await tripChatService.GetAllTripChatInfoByTripIdAsync(11);

            var json = tripChats.ConvertToJson();
            Console.WriteLine(json);
        }

        [TestMethod]
        public void GetCreateTripChatJson()
        {
            var tripService = ServiceProvider.GetRequiredService<TripService>();
            var trip = tripService.GetTripDtoByIdAsync(1).Result; // Adjust with a valid TripId for testing

            var tripChatService = ServiceProvider.GetRequiredService<TripChatService>();
            var tripChatDto = new TripChatDto
            {
                TripId = trip.TripId,
                Description = "Test Chat",
                Chat = "This is a test chat message."
            };

            var json = tripChatDto.ConvertToJson();
            Console.WriteLine(json);
        }

        [TestMethod]
        public void TestCreateTripChat()
        {
            var tripService = ServiceProvider.GetRequiredService<TripService>();
            var trip = tripService.GetTripDtoByIdAsync(11).Result; // Adjust with a valid TripId for testing

            var tripChatService = ServiceProvider.GetRequiredService<TripChatService>();
            var tripChatDto = new TripChatDto
            {
                TripId = trip.TripId,
                Description = "Test Chat",
                Chat = "This is a test chat message."
            };
            var addedTripChat = tripChatService.CreateTripChatAsync(tripChatDto).Result;
            Assert.IsNotNull(addedTripChat);
            Assert.IsTrue(addedTripChat.TripChatId > 0);
            Console.WriteLine(addedTripChat.ConvertToJson());
        }

        [TestMethod]
        public void TestUpdateTripChat()
        {
            var tripChatService = ServiceProvider.GetRequiredService<TripChatService>();
            var tripChat = tripChatService.GetByTripChatId(6).Result; // Adjust with a valid TripChatId for testing
            Assert.IsNotNull(tripChat);

            tripChat.Chat = "Updated chat message.";

            var updatedTripChat = tripChatService.UpdateTripChatAsync(tripChat).Result;
            Assert.IsNotNull(updatedTripChat);
            Assert.AreEqual("Updated chat message.", updatedTripChat.Chat);
            Console.WriteLine(updatedTripChat.ConvertToJson());
        }

        [TestMethod]
        public void TestDeleteTripChat()
        {
            var tripChatService = ServiceProvider.GetRequiredService<TripChatService>();
            var result = tripChatService.DeleteTripChatAsync(3).Result; // Adjust with a valid TripChatId for testing
            Assert.IsTrue(result);
            Console.WriteLine("TripChat deleted successfully.");
        }
    }
}
