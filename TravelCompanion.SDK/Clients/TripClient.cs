using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using TravelCompanion.Domain.DTOs;

namespace TravelCompanion.SDK.Clients
{
    public class TripClient
    {
        private readonly HttpClient _httpClient;

        public TripClient(HttpClient httpClient, IApiKeyProvider apiKeyProvider)
        {
            _httpClient = httpClient;
            // Add X-Api-Key header to all requests
            _httpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKeyProvider.GetApiKey());
        }

        /// <summary>
        /// Gets all trips for the current user, defined by the supplied API key
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TripDto>> GetAllTripsForCurrentUser()
        {
            var response = await _httpClient.GetAsync("api/trip");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<IEnumerable<TripDto>>();
        }

        public async Task<TripDto> GetTripByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"api/trip/{id}");
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripDto>();
        }

        public async Task<TripDto> CreateTripAsync(TripDto tripDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/trip", tripDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripDto>();
        }

        public async Task<TripDto> UpdateTripAsync(int id, TripDto tripDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/trip/{id}", tripDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TripDto>();
        }

        public async Task<bool> DeleteTripAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/trip/{id}");
            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }
    }
}
