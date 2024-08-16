using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TravelCompanion.Domain.DTOs;

namespace TravelCompanion.SDK.Clients
{
    public class TripEventClient
    {
        private readonly HttpClient _httpClient;

        public TripEventClient(HttpClient httpClient, IApiKeyProvider apiKeyProvider)
        {
            _httpClient = httpClient;
            // Add X-Api-Key header to all requests
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKeyProvider.GetApiKey());
        }

        /// <summary>
        /// Gets all TripEvents for a specific Trip by TripId.
        /// </summary>
        /// <param name="tripId">The ID of the trip.</param>
        /// <returns>A collection of TripEventDto.</returns>
        public async Task<IEnumerable<TripEventDto>> GetAllTripEventsForTripAsync(int tripId)
        {
            var response = await _httpClient.GetAsync($"api/tripevent/{tripId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<TripEventDto>>();
        }

        /// <summary>
        /// Gets a specific TripEvent by its ID.
        /// </summary>
        /// <param name="tripEventId">The ID of the TripEvent.</param>
        /// <returns>The TripEventDto object.</returns>
        public async Task<TripEventDto> GetTripEventByIdAsync(int tripEventId)
        {
            var response = await _httpClient.GetAsync($"api/tripevent/event/{tripEventId}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripEventDto>();
        }

        /// <summary>
        /// Creates a new TripEvent.
        /// </summary>
        /// <param name="tripEventDto">The TripEventDto object to create.</param>
        /// <returns>The created TripEventDto object.</returns>
        public async Task<TripEventDto> CreateTripEventAsync(TripEventDto tripEventDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tripevent", tripEventDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripEventDto>();
        }

        /// <summary>
        /// Updates an existing TripEvent by its ID.
        /// </summary>
        /// <param name="tripEventId">The ID of the TripEvent to update.</param>
        /// <param name="tripEventDto">The updated TripEventDto object.</param>
        /// <returns>The updated TripEventDto object.</returns>
        public async Task<TripEventDto> UpdateTripEventAsync(int tripEventId, TripEventDto tripEventDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tripevent/{tripEventId}", tripEventDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripEventDto>();
        }

        /// <summary>
        /// Deletes a TripEvent by its ID.
        /// </summary>
        /// <param name="tripEventId">The ID of the TripEvent to delete.</param>
        /// <returns>True if the delete was successful, false otherwise.</returns>
        public async Task<bool> DeleteTripEventAsync(int tripEventId)
        {
            var response = await _httpClient.DeleteAsync($"api/tripevent/{tripEventId}");
            return response.IsSuccessStatusCode;
        }
    }
}
