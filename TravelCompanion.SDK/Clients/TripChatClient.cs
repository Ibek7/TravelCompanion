using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TravelCompanion.Domain.DTOs;

namespace TravelCompanion.SDK.Clients
{
    public class TripChatClient
    {
        private readonly HttpClient _httpClient;

        public TripChatClient(HttpClient httpClient, IApiKeyProvider apiKeyProvider)
        {
            _httpClient = httpClient;
            // Add X-Api-Key header to all requests
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKeyProvider.GetApiKey());
        }

        /// <summary>
        /// Gets all TripChats for a specific Trip by TripId.
        /// </summary>
        /// <param name="tripId">The ID of the trip.</param>
        /// <returns>A collection of TripChatInfoDto.</returns>
        public async Task<IEnumerable<TripChatInfoDto>> GetAllTripChatsForTripAsync(int tripId)
        {
            var response = await _httpClient.GetAsync($"api/tripchat/{tripId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<TripChatInfoDto>>();
        }

        /// <summary>
        /// Gets a specific TripChat by its ID.
        /// </summary>
        /// <param name="tripChatId">The ID of the TripChat.</param>
        /// <returns>The TripChatInfoDto object.</returns>
        public async Task<TripChatDto> GetTripChatByIdAsync(int tripChatId)
        {
            var response = await _httpClient.GetAsync($"api/tripchat/chat/{tripChatId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripChatDto>();
        }

        /// <summary>
        /// Creates a new TripChat.
        /// </summary>
        /// <param name="tripChatDto">The TripChatDto object to create.</param>
        /// <returns>The created TripChatDto object.</returns>
        public async Task<TripChatDto> CreateTripChatAsync(TripChatDto tripChatDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tripchat", tripChatDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripChatDto>();
        }

        /// <summary>
        /// Updates an existing TripChat by its ID.
        /// </summary>
        /// <param name="tripChatId">The ID of the TripChat to update.</param>
        /// <param name="tripChatDto">The updated TripChatDto object.</param>
        /// <returns>The updated TripChatDto object.</returns>
        public async Task<TripChatDto> UpdateTripChatAsync(int tripChatId, TripChatDto tripChatDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tripchat/{tripChatId}", tripChatDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripChatDto>();
        }

        /// <summary>
        /// Deletes a TripChat by its ID.
        /// </summary>
        /// <param name="tripChatId">The ID of the TripChat to delete.</param>
        /// <returns>True if the delete was successful, false otherwise.</returns>
        public async Task<bool> DeleteTripChatAsync(int tripChatId)
        {
            var response = await _httpClient.DeleteAsync($"api/tripchat/{tripChatId}");
            return response.IsSuccessStatusCode;
        }
    }
}
