using Microsoft.Extensions.DependencyInjection;
using TravelCompanion.Domain.DTOs;
using TravelCompanion.Domain.Extensions;
using TravelCompanion.SDK.Clients;

namespace TravelCompanion.Tests
{
    /// <summary>
    /// Goes through the SDK to test the TripChatClient. The SDK will communicate through the
    /// web API with connection string defined in Configuration["TravelCompanionBaseAddress"].
    /// </summary>
    [TestClass]
    public class TripChatClientSdkTests : BaseTests
    {
        private TripChatClient _tripChatClient;

        [TestInitialize]
        public void Setup()
        {
            _tripChatClient = ServiceProvider.GetRequiredService<TripChatClient>();
        }

        [TestMethod]
        public async Task TestGetAllTripChatsForTrip()
        {
            var tripId = 11; // Adjust with a valid TripId for testing
            var tripChats = await _tripChatClient.GetAllTripChatsForTripAsync(tripId);
            Assert.IsNotNull(tripChats);
            Assert.IsTrue(tripChats.Any());
            Console.WriteLine(tripChats.ConvertToJson());
        }

        [TestMethod]
        public async Task TestGetTripChatById()
        {
            var tripChatId = 1; // Adjust with a valid TripChatId for testing
            var tripChat = await _tripChatClient.GetTripChatByIdAsync(tripChatId);
            Assert.IsNotNull(tripChat);
            Console.WriteLine(tripChat.ConvertToJson());
        }

        [TestMethod]
        public async Task TestCreateTripChat()
        {
            var newTripChat = new TripChatDto
            {
                TripId = 11, // Adjust with a valid TripId for testing
                Description = "Test Chat",
                Chat = "This is a test chat message."
            };

            var createdTripChat = await _tripChatClient.CreateTripChatAsync(newTripChat);
            Assert.IsNotNull(createdTripChat);
            Assert.AreEqual(newTripChat.Description, createdTripChat.Description);
            Console.WriteLine(createdTripChat.ConvertToJson());
        }

        [TestMethod]
        public async Task TestUpdateTripChat()
        {
            var tripChatId = 1; // Adjust with a valid TripChatId for testing
            var tripChat = await _tripChatClient.GetTripChatByIdAsync(tripChatId);
            Assert.IsNotNull(tripChat);

            tripChat.Chat = "Updated chat message.";

            var updatedTripChat = await _tripChatClient.UpdateTripChatAsync(tripChatId, tripChat);
            Assert.IsNotNull(updatedTripChat);
            Assert.AreEqual("Updated chat message.", updatedTripChat.Chat);
            Console.WriteLine(updatedTripChat.ConvertToJson());
        }

        [TestMethod]
        public async Task TestDeleteTripChat()
        {
            var tripChatId = 1; // Adjust with a valid TripChatId for testing
            var success = await _tripChatClient.DeleteTripChatAsync(tripChatId);
            Assert.IsTrue(success);
            Console.WriteLine($"TripChat with ID {tripChatId} deleted successfully.");
        }
    }
}
